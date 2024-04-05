using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotMover))] 
public class Bot : MonoBehaviour
{
    [SerializeField] Base _baseTemplate;

    private BotMover _mover;
    private Queue<Mineral> _targetMinerals;
    private Base _base;
    private bool _isCollectionInProgress;
    private bool _isColonizationInProgress;

    public Transform Slot { get; private set; }
    public bool IsFree => _targetMinerals.Count == 0;
    public BotMover Mover => _mover;

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
        _targetMinerals = new Queue<Mineral>();         
    }

    private void Update()
    {
        if(_isColonizationInProgress == false &&
           _targetMinerals.Count != 0 && 
           _isCollectionInProgress == false)
        {
            StartCoroutine(Collect(_targetMinerals.Peek()));            
        }        
    }

    public void Init(Base mineralBase, Transform slot)
    {
        _base = mineralBase;
        Slot = slot;
    }

    public void AddTarget(Mineral target)
    {               
        _targetMinerals.Enqueue(target);        
    }

    public bool ContainsTarget(Mineral target)
    {
        return _targetMinerals.Contains(target);
    }

    public void ClearTargets()
    {
        if (_targetMinerals.Count != 0)
            _targetMinerals.Clear();
    }

    public IEnumerator Colonize(Flag flag)
    {
        _isColonizationInProgress = true;

        yield return StartCoroutine(_mover.MoveTo(flag.transform));
        Destroy(flag.gameObject);
        StandBase();

        _isColonizationInProgress = false;
    }

    private void StandBase()
    {
        Base newBase = Instantiate(_baseTemplate, transform.position, Quaternion.identity);
        _base.Level.AddBase(newBase);
        _base.Level.ClearAllTargets();
        newBase.Init(_base.Level, this, _base.InfoPanel);
    }

    private IEnumerator Collect(Mineral mineral)
    {
        _isCollectionInProgress = true;

        yield return StartCoroutine(_mover.MoveTo(mineral.transform));
        yield return StartCoroutine(_mover.MoveTo(_base.transform));
        yield return StartCoroutine(_mover.MoveReverse());

       
        if (_targetMinerals.Count != 0)
            _targetMinerals.Dequeue();        

        if (IsFree)
        {
            yield return StartCoroutine(_mover.MoveTo(Slot));
            yield return StartCoroutine(_mover.TurnTo(_base.transform));
        }        

        _isCollectionInProgress = false;
    }
}
