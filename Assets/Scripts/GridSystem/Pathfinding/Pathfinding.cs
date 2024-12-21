using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathfindingNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)//check if there are more than one UnitActionSystem
        {
            Debug.LogError("There's more than one Pathfinding! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closeList = new List<PathfindingNode>();

        PathfindingNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathfindingNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathfindingNode pathfindingNode = gridSystem.GetGridObject(gridPosition);

                pathfindingNode.SetACost(int.MaxValue);
                pathfindingNode.SetBCost(0);
                pathfindingNode.CalculateCCost();
                pathfindingNode.ResetComeFromPathfindingNode();
            }
        }

        startNode.SetACost(0);
        startNode.SetBCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateCCost();

        while (openList.Count > 0)
        {
            PathfindingNode currentNode = GetLowestCCostPathfindingNode(openList);

            if (currentNode == endNode)
            {
                // Reached final node
                pathLength = endNode.GetCCost();
                return CalculatedPath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathfindingNode closedByNode in GetClosedByList(currentNode))
            {
                if (closeList.Contains(closedByNode))
                {
                    continue;
                }

                if (!closedByNode.IsWalkable())
                {
                    closeList.Add(closedByNode);
                    continue;
                }

                int tentativeACost = currentNode.GetACost() + CalculateDistance(currentNode.GetGridPosition(), closedByNode.GetGridPosition());

                if (tentativeACost < closedByNode.GetACost())
                {
                    closedByNode.SetComeFromPathfindingNode(currentNode);
                    closedByNode.SetACost(tentativeACost);
                    closedByNode.SetBCost(CalculateDistance(closedByNode.GetGridPosition(), endGridPosition));
                    closedByNode.CalculateCCost();

                    if (!openList.Contains(closedByNode))
                    {
                        openList.Add(closedByNode);
                    }
                }
            }
        }

        // No path found
        pathLength = 0;
        return null;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathfindingNode>(width, height, cellSize, (GridSystem<PathfindingNode> a, GridPosition gridPosition) => new PathfindingNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPositon = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float rayCastOffSetDistance = 5f;
                if (Physics.Raycast(worldPositon + Vector3.down * rayCastOffSetDistance, Vector3.up, rayCastOffSetDistance * 2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int distance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathfindingNode GetLowestCCostPathfindingNode(List<PathfindingNode> pathfindingNodeList)
    {
        PathfindingNode lowestCCostPathNode = pathfindingNodeList[0];
        for (int i = 0; i < pathfindingNodeList.Count; i++)
        {
            if (pathfindingNodeList[i].GetCCost() < lowestCCostPathNode.GetCCost())
            {
                lowestCCostPathNode = pathfindingNodeList[i];
            }
        }
        return lowestCCostPathNode;
    }

    private PathfindingNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathfindingNode> GetClosedByList(PathfindingNode currentNode)
    {
        List<PathfindingNode> closedByList = new List<PathfindingNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            //Left Node
            closedByList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //Left Down Node
                closedByList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up Node
                closedByList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right Node
            closedByList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                //Right Down Node
                closedByList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up Node
                closedByList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up Node
            closedByList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        if (gridPosition.z - 1 >= 0)
        {
            //Down Node
            closedByList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        return closedByList;
    }

    private List<GridPosition> CalculatedPath(PathfindingNode endNode)
    {
        List<PathfindingNode> pathfindingNodeList = new List<PathfindingNode>();
        pathfindingNodeList.Add(endNode);
        PathfindingNode currentNode = endNode;

        while (currentNode.GetComeFromPathfindingNode() != null)
        {
            pathfindingNodeList.Add(currentNode.GetComeFromPathfindingNode());
            currentNode = currentNode.GetComeFromPathfindingNode();
        }

        pathfindingNodeList.Reverse();

        List<GridPosition> gridPositionsList = new List<GridPosition>();
        foreach (PathfindingNode node in pathfindingNodeList)
        {
            gridPositionsList.Add(node.GetGridPosition());
        }

        return gridPositionsList;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool AvailablePath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
