using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMode : MonoBehaviour
{
    [SerializeField] protected Base Base;
    [SerializeField] protected int Price;

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
            Execute();
        }
    }
}
