using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBurstAction : UnitActionBase
{
    [SerializeField] private Transform spellProjectilePrefab;
    private int maxSpellDistance = 6;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }
    public override string GetActionName()
    {
        return "Spell Burst";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSpellDistance; x <= maxSpellDistance; x++)
        {
            for (int z = -maxSpellDistance; z <= maxSpellDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //Make the shooting range in circle around the character(much more diamonds shape)
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxSpellDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
                Debug.Log("Spell Distance: " + testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform spellProjectileTransform = Instantiate(spellProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        SpellBurstProjectile spellBurstProjectile = spellProjectileTransform.GetComponent<SpellBurstProjectile>();
        spellBurstProjectile.Setup(gridPosition, OnSpellBurstComplete);

        ActionInit(onActionComplete);
    }

    private void OnSpellBurstComplete()
    {
        ActionFinish();
    }
}
