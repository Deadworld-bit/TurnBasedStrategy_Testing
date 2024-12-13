using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathfindingNode> gridSystem;

    private void Awake()
    {
        gridSystem = new GridSystem<PathfindingNode>(10, 10, 2f, (GridSystem<PathfindingNode> a, GridPosition gridPosition) => new PathfindingNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
}
