using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : UnitActionBase
{
    // public delegate void SpinCompleteDelegate();   Define delegate myself
    private float totalSpinAmount;
    // private SpinCompleteDelegate onSpinComplete;   Delegate :)))
    // private Action onSpinComplete;                 using C# build in delegate

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
    
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionFinish();
        }
    }

    // public void Spin(SpinCompleteDelegate onSpinComplete)     
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0f;
        ActionInit(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}