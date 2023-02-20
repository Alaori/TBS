using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : UnitAction
{
    [SerializeField] private int maxMoveDistance = 4;
    public event EventHandler onStartMoving;
    public event EventHandler onStopMoving;

    private List<Vector3> targetPositionList;
    private int index;
    // Start is called before the first frame update
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        Vector3 targetPosition = targetPositionList[index];
     
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

        }
        else
        {
            index++;
            if (index >= targetPositionList.Count)
            {
                if (onStopMoving != null)
                {
                    onStopMoving(this, EventArgs.Empty);
                }
                ActionComplete();
            }
        }

    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLenght);
        index = 0;
        targetPositionList = new List<Vector3>();
        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            targetPositionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        if (onStartMoving != null)
        {
            onStartMoving(this, EventArgs.Empty);
        }
        ActionStart(onActionComplete);

    }
    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                if(LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxMoveDistance)
                {
                    continue;
                }
                if(!PathFinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!PathFinding.Instance.HasPath(unitGridPosition,testGridPosition))
                {
                    continue;
                }
                int pathFindingDistanceMultiplier = 10;
                if (PathFinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathFindingDistanceMultiplier)
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);

            }
        }
      return validGridPositionList;

    }
   
    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCount = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10,
        };
    }
}

