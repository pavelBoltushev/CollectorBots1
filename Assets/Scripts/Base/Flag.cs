using System.Collections;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private float _blinkPeriodicity;

    private MeshRenderer[] _renderers;

    private void Start()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        var wait = new WaitForSeconds(_blinkPeriodicity);
        Color startColor = _renderers[0].material.color;

        while (true)
        {
            SetColor(Color.white);
            yield return wait;
            SetColor(startColor);
            yield return wait;
        }
    }

    private void SetColor(Color color)
    {
        foreach (var renderer in _renderers)
        {
            renderer.material.color = color;
        }
    }
}
