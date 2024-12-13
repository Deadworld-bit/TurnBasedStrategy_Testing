using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;             // this also delegate :)))
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStart;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private bool isBusy;
    private UnitActionBase selectedAction;

    private void Awake()
    {
        if (Instance != null)//check if there are more than one UnitActionSystem
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection()) return;
        HandleSelectedAction();

        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (TryHandleUnitSelection()) return;//handle unit selection once not evey frame of the game

        //     GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        //     if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
        //     {
        //         SetBusy();
        //         selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
        //     }
        // }

        // if (Input.GetMouseButtonDown(1))
        // {
        //     SetBusy();
        //     selectedUnit.GetSpinAction().Spin(ClearBusy);
        // }
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedUnit.TryConsumeActionPointsToTakeAction(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);

                    OnActionStart?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            // Using switch for each action instead of generic one
            // switch (selectedAction)
            // {
            //     case MoveAction moveAction:
            //         if (moveAction.IsValidActionGridPosition(mouseGridPosition))
            //         {
            //             SetBusy();
            //             moveAction.Move(mouseGridPosition, ClearBusy);
            //         }
            //         break;
            //     case SpinAction spinAction:
            //         SetBusy();
            //         spinAction.Spin(ClearBusy);
            //         break;
            // }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))// dont have to do null test
                {
                    if (unit == selectedUnit)
                    {
                        //this unit is already been selected duh
                        return false;
                    }

                    if (unit.IsEnemy())
                    {
                        //Check if the selected unit is enemy or not bruh
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        // if (OnSelectedUnitChanged != null)
        // {
        //     OnSelectedUnitChanged(this, EventArgs.Empty);
        // }
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(UnitActionBase actionBase)
    {
        selectedAction = actionBase;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public UnitActionBase GetSelectedAction()
    {
        return selectedAction;
    }

}
