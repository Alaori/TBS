using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }

    private int length;
    private int width;
    private float cellSize;
    private const int straightCost = 10;
    private const int diagonalCost = 14; // square rt 2

    [SerializeField] private Transform db;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private GridSystem<PathNode> gridSystem;
    private void Awake()
    {
       if (Instance != null)
       {
            Debug.Log("More than 1 instance");
            Destroy(gameObject);
            return;
       }
       Instance = this;
       
       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Setup(int length, int width, float cellSize)
    {
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;
        gridSystem = new GridSystem<PathNode>(width, length, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
       // gridSystem.cDebug(db);

       for(int x =0; x < width;x++)
       { 
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float rayCastDistance = 5f;
                if(Physics.Raycast(worldPosition + Vector3.down* rayCastDistance, Vector3.up, rayCastDistance *2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);

                }
            }
       }

    }
    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition, out int pathLenght)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPosition);
        PathNode endNode = gridSystem.GetGridObject(endPosition);

        openList.Add(startNode);
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetLength(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousPathNode();

            }
        }
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition, endPosition));
        startNode.CalculateFCost();
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if (currentNode == endNode)
            {
                pathLenght = endNode.GetFCost();
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            foreach(PathNode neighbourNode in GetNeigbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if(!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int currentGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                if(currentGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPreviousPathNode(currentNode);
                    neighbourNode.SetGCost(currentGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPosition));
                    neighbourNode.CalculateFCost();
                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        pathLenght = 0;
        return null;
    }
    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a - b;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return diagonalCost * Mathf.Min(xDistance,zDistance) + straightCost * remaining;
    }
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }
    private List<PathNode> GetNeigbourList(PathNode currentNode)
    {
        List<PathNode> neigbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();
        if (gridPosition.x - 1 >= 0)
        {
            //left
            neigbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0)
            {
                //left down
                neigbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1  < gridSystem.GetLength())
            {  //left up
                neigbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //right
            neigbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0)
            {
                //right down
                neigbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetLength())
            {
                //right up      
                neigbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.z + 1 < gridSystem.GetLength())
        {   //up
            neigbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1));
        }
        if (gridPosition.z - 1 >= 0)
        {
            //down
            neigbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1));
        }
        return neigbourList;
    }
    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetPreviousPathNode() !=null)
        {
            pathNodeList.Add(currentNode.GetPreviousPathNode());
            currentNode = currentNode.GetPreviousPathNode();
        }
        pathNodeList.Reverse();
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }
    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }
    public bool HasPath(GridPosition startPosition, GridPosition endPosition)
    {
        if (FindPath(startPosition, endPosition,out int pathLenght) ==null)
        {
            return false;
        }
        return true;

    }
    public int GetPathLength(GridPosition startPosition, GridPosition endPosition)
    {
        FindPath(startPosition, endPosition, out int pathLength);
        return pathLength;

    }
}
