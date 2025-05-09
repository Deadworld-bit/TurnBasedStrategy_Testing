using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;

public class ShootArrowAction : UnitActionBase
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;

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

    [SerializeField] private LayerMask obstacleLayerMask;

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
                float shootingStateTime = 2f;
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
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPosition(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPosition(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

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

                //Check to not shoot through walls
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = targetUnit.GetWorldPosition() - unitWorldPosition.normalized;
                float unitShoulderHeight = 1.5f;

                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight,
                shootDir, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                obstacleLayerMask))
                {
                    //Blocked by obstacle
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
                Debug.Log("Shoot Distance: " + testGridPosition);
            }
        }

        return validGridPositionList;
    }

    // private void Shoot()
    // {
    //     OnShoot?.Invoke(this,new OnShootEventArgs
    //     {
    //         targetUnit = targetUnit,
    //         shootingUnit = unit
    //     });
    //     targetUnit.Damage(30);
    // }

    private async void Shoot()
    {
        // Invoking the OnShoot event
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        
        await Task.Delay(1050);
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        // Adding a delay before applying damage
        await Task.Delay(600); // Delay for 1000 milliseconds (1 second) 
        targetUnit.Damage(30);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 0.75f;
        stateTimer = aimingStateTime;
        fire = true;

        ActionInit(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthPointNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPosition(gridPosition).Count;
    }
}