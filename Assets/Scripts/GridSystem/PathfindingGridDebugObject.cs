using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro aCostText;
    [SerializeField] private TextMeshPro bCostText;
    [SerializeField] private TextMeshPro cCostText;

    private PathfindingNode pathfindingNode;

    public override void SetGridObject(object gridObject)
    {
        pathfindingNode = (PathfindingNode)gridObject;
        base.SetGridObject(gridObject);
    }

    protected override void Update()
    {
        base.Update();
        aCostText.text = pathfindingNode.GetACost().ToString();
        bCostText.text = pathfindingNode.GetBCost().ToString();
        cCostText.text = pathfindingNode.GetCCost().ToString();
    }
}
