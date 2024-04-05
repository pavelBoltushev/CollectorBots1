using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float _transferTargetsPeriodicity;
    [SerializeField] private List<Base> _bases;

    private List<Mineral> _minerals = new List<Mineral>();

    private void Start()
    {
        foreach (var oneBase in _bases)
        {
            oneBase.SetLevel(this);
        }

        StartCoroutine(TransferTargets());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _minerals.Add(mineral);
        }
    }    

    public void RemoveMineral(Mineral mineral)
    {
        _minerals.Remove(mineral);
    }

    public void AddBase(Base addedBase)
    {
        _bases.Add(addedBase);
    }

    public void ClearAllTargets()
    {
        foreach (var currentBase in _bases)
        {
            currentBase.ClearAllBotsTargets();
        }
    }

    private IEnumerator TransferTargets()
    {
        var wait = new WaitForSeconds(_transferTargetsPeriodicity);

        while (true)
        {
            foreach (var mineral in _minerals)
            {
                if(mineral != null)
                {
                    _bases.OrderBy(currentBase => Vector3.Distance(mineral.transform.position, currentBase.transform.position))
                            .First()
                            .TransferTarget(mineral);
                }
            }            

            yield return wait;
        }
    }
}
