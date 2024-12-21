using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : UnitActionBase
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    // [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 5;

    private List<Vector3> targetPositionList;
    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = targetPositionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // transform.forward=moveDirection;   the most simple way to make the character ratate to moving direction
        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) //Make the character stop moving when reach position
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            // unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= targetPositionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionFinish();
            }
            // unitAnimator.SetBool("isWalking", false);
            // OnStopMoving?.Invoke(this, EventArgs.Empty);
            // ActionFinish();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathfindingGridPositions = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
        currentPositionIndex = 0;
        targetPositionList = new List<Vector3>();

        foreach (GridPosition pathfindingGridPosition in pathfindingGridPositions)
        {
            targetPositionList.Add(LevelGrid.Instance.GetWorldPosition(pathfindingGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionInit(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position already occupied with another Unit
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!Pathfinding.Instance.AvailablePath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    //The path is way too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
                Debug.Log("Movable Place: " + testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootArrowAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}