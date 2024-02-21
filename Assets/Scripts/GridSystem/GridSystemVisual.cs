using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    
    public enum GridVisualType
    {
        White,
        Green,
        Red,
        LightRed,
        Yellow
    }
    
    [SerializeField] private Transform gridSystemVisualSlotPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialsList;

    private GridSystemVisualTitle[,] gridSystemVisualTitleArray;

    private void Awake()
    {
        if (Instance != null)//check if there are more than one GridSystemVisual
        {
            Debug.LogError("There's more than one GridSystemVisual! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualTitleArray = new GridSystemVisualTitle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform gridSytemVisualSingleTransform = Instantiate(gridSystemVisualSlotPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualTitleArray[x, z] = gridSytemVisualSingleTransform.GetComponent<GridSystemVisualTitle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovePosition += LevelGrid_OnAnyUnitMovePosition;
        
        UpdateGridVisual();
    }
    
    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualTitleArray[x, z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x= -range; x <= range; x++ )
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }
        
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    
    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualTitleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        UnitActionBase selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;
        switch (selectedAction)
        {
            default:
            case MoveAction moveAction :
                gridVisualType = GridVisualType.White;
                break;
            
            case SpinAction spinAction :
                gridVisualType = GridVisualType.Green;
                break;
            
            case ShootArrowAction shootArrowAction :
                gridVisualType = GridVisualType.Red;
                
                //Show range of shooting arrow
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootArrowAction.GetMaxShootDistance(), GridVisualType.LightRed);
                break;
        }
        
        ShowGridPositionList(selectedAction.GetValidActionGridPosition(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovePosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialsList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        
        Debug.Log("Could not find GridVisualTypeMaterial for GridVisualType "+ gridVisualType);
        return null;
    }
}
