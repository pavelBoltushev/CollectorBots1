using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseController), typeof(BaseView))]
public abstract class BaseMode : MonoBehaviour
{
    [SerializeField] protected Base Base;
    [SerializeField] protected int Price;

    private BaseView _view;
    private BaseController _controller;

    private void Awake()
    {
        _view = GetComponent<BaseView>();
        _controller = GetComponent<BaseController>();
    }

    private void OnEnable()
    {
        Base.MineralCountChanged += Check;
    }

    private void OnDisable()
    {
        Base.MineralCountChanged -= Check;
    }

    protected abstract void Execute();    

    private void Check(int mineralCount)
    {
        if(mineralCount >= Price)
        {
            Base.SpendMinerals(Price);

            if(_controller.IsClicked)
                _view.InfoPanel.SetMineralCount(Base.MineralsCount);

            Execute();
        }
    }
}
