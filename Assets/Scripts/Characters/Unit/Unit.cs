using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 4;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDown;

    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootArrowAction shootArrowAction;

    private HealthPointSystem healthPointSystem;
    private int actionPoints = ACTION_POINTS_MAX;

    private UnitActionBase[] baseActionArray;

    private void Awake()
    {
        healthPointSystem = GetComponent<HealthPointSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootArrowAction = GetComponent<ShootArrowAction>();
        baseActionArray = GetComponents<UnitActionBase>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthPointSystem.OnUnitDown += HealthPointSystem_OnUnitDown;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public ShootArrowAction GetShootArrowAction(){
        return shootArrowAction;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public UnitActionBase[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TryConsumeActionPointsToTakeAction(UnitActionBase actionBase)
    {
        if (ConsumeActionPointsToTakeAction(actionBase))
        {
            ConsumeActionPoints(actionBase.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ConsumeActionPointsToTakeAction(UnitActionBase actionBase)
    {
        if (actionPoints >= actionBase.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ConsumeActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthPointSystem.Damage(damageAmount);
        Debug.Log(transform + "Target damaged!");
    }

    private void HealthPointSystem_OnUnitDown(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDown?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthPointNormalized(){
        return healthPointSystem.getHealthPercentage();
    }
}