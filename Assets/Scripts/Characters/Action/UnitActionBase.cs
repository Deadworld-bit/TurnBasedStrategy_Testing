using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitActionBase : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    protected void ActionInit(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionFinish()
    {
        isActive = false;
        onActionComplete();
        
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public abstract string GetActionName();

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPosition();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPosition();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    public Unit GetUnit()
    {
        return unit;
    }
}