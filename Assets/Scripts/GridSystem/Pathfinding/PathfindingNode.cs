using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private GridPosition gridPosition;
    private int aCost; //g
    private int bCost; //h
    private int cCost; //f

    private PathfindingNode comeFromPathfindingNode;
    private bool isWalkable = true;

    public PathfindingNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetACost()
    {
        return aCost;
    }

    public int GetBCost()
    {
        return bCost;
    }

    public int GetCCost()
    {
        return cCost;
    }

    public void SetACost(int aCost)
    {
        this.aCost = aCost;
    }

    public void SetBCost(int bCost)
    {
        this.bCost = bCost;
    }

    public void CalculateCCost()
    {
        cCost = aCost + bCost;
    }

    public void ResetComeFromPathfindingNode()
    {
        comeFromPathfindingNode = null;
    }

    public void SetComeFromPathfindingNode(PathfindingNode pathfindingNode)
    {
        comeFromPathfindingNode = pathfindingNode;
    }

    public PathfindingNode GetComeFromPathfindingNode()
    {
        return comeFromPathfindingNode;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
