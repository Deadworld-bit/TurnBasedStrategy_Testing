using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private float timer;
    private State state;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeAIEnemyAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        //No more enemies to take turn
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 3f;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeAIEnemyAction(Action onAIEnemyActionComplete)
    {
        // Debug.Log("Take AI Action");
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeAIEnemyAction(enemyUnit, onAIEnemyActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeAIEnemyAction(Unit enemyUnit, Action onAIEnemyActionComplete)
    {
        EnemyAIAction suitableEnemyAIAction = null;
        UnitActionBase suitableBaseAction = null;
        foreach (UnitActionBase baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.ConsumeActionPointsToTakeAction(baseAction))
            {
                //Enemy cannot do this action !!!
                continue;
            }

            if (suitableEnemyAIAction == null)
            {
                suitableEnemyAIAction = baseAction.GetSuitableEnemyAIAction();
                suitableBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetSuitableEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > suitableEnemyAIAction.actionValue)
                {
                    suitableEnemyAIAction = testEnemyAIAction;
                    suitableBaseAction = baseAction;
                }
            }

        }

        if (suitableEnemyAIAction != null && enemyUnit.TryConsumeActionPointsToTakeAction(suitableBaseAction))
        {
            suitableBaseAction.TakeAction(suitableEnemyAIAction.gridPosition, onAIEnemyActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
