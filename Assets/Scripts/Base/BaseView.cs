using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BaseView : MonoBehaviour
{        
    [SerializeField] private Color _markingColor;

    private MeshRenderer _renderer;
    private Color _startColor;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _startColor = _renderer.material.color;
    }

    public void Mark()
    {
        _renderer.material.color = _markingColor;
    }

    public void Unmark()
    {
        _renderer.material.color = _startColor;
    }
}
