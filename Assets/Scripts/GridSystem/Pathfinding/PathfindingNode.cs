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
}
