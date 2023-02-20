using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode previousNode;
    private bool walkable = true;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }
   public int GetGCost()
    {
        return gCost;
    }
    public int GetFCost()
    {
        return fCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void ResetPreviousPathNode()
    {
        previousNode = null;
    }
    public GridPosition GetGridPosition()
    {

        return gridPosition;
    }
    public void SetPreviousPathNode(PathNode pathNode)
    {
        previousNode = pathNode;
    }
    public PathNode GetPreviousPathNode()
    {
        return previousNode;
    }
    public bool IsWalkable()
    {
        return walkable;
    }
    public void SetIsWalkable(bool walkable)
    {
        this.walkable = walkable;
    }
}
