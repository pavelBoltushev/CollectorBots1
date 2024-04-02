using UnityEngine;
using System.Linq;

public class Base : MonoBehaviour
{
    [SerializeField] CollectorBot[] _bots;

    private void Start()
    {
        foreach (var bot in _bots)
        {
            bot.SetBase(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {            
            _bots.OrderBy(bot => Vector3.Distance(bot.StartPosition, mineral.transform.position))
                 .First()
                 .AddTarget(mineral);
        }
    }
}
