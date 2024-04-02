using UnityEngine;
using UnityEngine.UI;

public class StorageTrigger : MonoBehaviour
{
    [SerializeField] private Text _mineralsCountText;

    private int _mineralsCount;

    private void Start()
    {
        _mineralsCountText.text = _mineralsCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _mineralsCount++;
            _mineralsCountText.text = _mineralsCount.ToString();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Mineral mineral))
        {
            _mineralsCount--;
            _mineralsCountText.text = _mineralsCount.ToString();
        }
    }
}
