using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotMover))] 
public class CollectorBot : MonoBehaviour
{
    private BotMover _mover;
    private Queue<Mineral> _targetMinerals;
    private Base _base;
    private bool _isCollectionInProgress;

    public Vector3 StartPosition { get; private set; }

    private void Start()
    {
        _mover = GetComponent<BotMover>();
        _targetMinerals = new Queue<Mineral>();
        StartPosition = transform.position;        
    }

    private void Update()
    {
        if(_targetMinerals.Count != 0 && _isCollectionInProgress == false)
        {
            StartCoroutine(Collect(_targetMinerals.Dequeue()));
        }
    }

    public void AddTarget(Mineral target)
    {
        _targetMinerals.Enqueue(target);
    }

    public void SetBase(Base mineralBase)
    {
        _base = mineralBase;
    }

    private IEnumerator Collect(Mineral mineral)
    {
        _isCollectionInProgress = true;

        yield return StartCoroutine(_mover.MoveTo(mineral.transform));
        yield return StartCoroutine(_mover.MoveTo(_base.transform));
        yield return StartCoroutine(_mover.MoveReverse());

        _isCollectionInProgress = false;
    }
}
