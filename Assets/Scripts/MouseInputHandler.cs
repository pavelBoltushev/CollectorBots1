using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private BaseController _clickedBase;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseClick();
    }

    private void OnMouseClick()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out BaseController clickedBase))
            {
                if (_clickedBase == clickedBase)
                {
                    _clickedBase.OnUnclicked();
                    _clickedBase = null;
                }
                else
                {
                    if(_clickedBase != null)
                        _clickedBase.OnUnclicked();

                    _clickedBase = clickedBase;
                    _clickedBase.OnClicked();
                }
            }
            else
            {
                if (_clickedBase != null)
                    _clickedBase.StandFlag(hit.point);
            }
        }
    }
}
