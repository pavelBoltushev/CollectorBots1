using UnityEngine;

[RequireComponent(typeof(Base), typeof(BaseView))]
public class BaseController : MonoBehaviour
{
    [SerializeField] private Flag _flagTemplate;
    [SerializeField] private float _minDistanceForFlag;

    private Base _base;
    private BaseView _baseView;

    public bool IsClicked { get; private set; }

    private void Awake()
    {
        _base = GetComponent<Base>();
        _baseView = GetComponent<BaseView>();
    }

    public void OnClicked()
    {
        IsClicked = true;
        _baseView.Mark();
        _baseView.InfoPanel.On();
        _baseView.InfoPanel.SetMineralCount(_base.MineralsCount);
        _baseView.InfoPanel.SetModeTypeText(_base.GetModeTypeText());
        _base.MineralCountChanged += _baseView.InfoPanel.SetMineralCount;
        _base.ModeChanged += _baseView.InfoPanel.SetModeTypeText;
    }

    public void OnUnclicked()
    {
        IsClicked = false;
        _baseView.Unmark();
        _baseView.InfoPanel.Off();
        _base.MineralCountChanged -= _baseView.InfoPanel.SetMineralCount;
        _base.ModeChanged -= _baseView.InfoPanel.SetModeTypeText;
    }

    public void StandFlag(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) > _minDistanceForFlag)
        {
            _base.Set(Mode.Colonization);            

            if (_base.Flag != null)
                Destroy(_base.Flag.gameObject);

            _base.Flag = Instantiate(_flagTemplate, position, Quaternion.identity);
        }
    }
}
