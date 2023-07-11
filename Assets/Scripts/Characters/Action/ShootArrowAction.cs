using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShootArrowAction : UnitActionBase
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    
    private enum State
    {
        Aiming,
        Shooting,
        Delay,
    }

    private State state;
    private int maxShootDistance = 4;
    private float stateTimer;
    private Unit targetUnit;
    private bool fire;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotationSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotationSpeed);
                break;
            case State.Shooting:
                if (fire)
                {
                    Shoot();
                    fire = false;
                }

                break;
            case State.Delay:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 5f;
                stateTimer = shootingStateTime;
                break;

            case State.Shooting:
                state = State.Delay;
                float delayStateTime = 0.5f;
                stateTimer = delayStateTime;
                break;

            case State.Delay:
                ActionFinish();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot Arrow";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //Make the shooting range in circle around the character(much more diamonds shape)
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no unit sighted :))
                    continue;
                }

                //Get unit at the test grid position 
                Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //Both unit on the same team
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
                Debug.Log("Shoot Distance: " + testGridPosition);
            }
        }

        return validGridPositionList;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this,new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionInit(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        fire = true;
    }
}