using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BaseInfoPanel : MonoBehaviour
{
    [SerializeField] private Text _mineralsCountText;
    [SerializeField] private Text _modeTypeText;

    private Image _panel;
    private Text[] _texts;

    private void Awake()
    {
        _panel = GetComponent<Image>();
        _texts = GetComponentsInChildren<Text>();
    }

    public void On()
    {
        _panel.enabled = true;

        foreach (var text in _texts)
        {
            text.enabled = true;
        }
    }

    public void Off()
    {
        _panel.enabled = false;

        foreach (var text in _texts)
        {
            text.enabled = false;
        }
    }

    public void SetMineralCount(int count)
    {
        _mineralsCountText.text = count.ToString();
    }

    public void SetModeTypeText(string modeType)
    {
        _modeTypeText.text = modeType; 
    }
}
