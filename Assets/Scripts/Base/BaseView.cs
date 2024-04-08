using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BaseView : MonoBehaviour
{
    [SerializeField] private BaseInfoPanel _infoPanel;
    [SerializeField] private Color _markingColor;

    private MeshRenderer _renderer;
    private Color _startColor;

    public BaseInfoPanel InfoPanel => _infoPanel;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _startColor = _renderer.material.color;
    }

    public void SetInfoPanel(BaseInfoPanel panel)
    {
        _infoPanel = panel;
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
