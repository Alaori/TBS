using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : UnitAction
{

    private int interactDistance = 1;
    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 50,
        };
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int x = -interactDistance; x <= interactDistance; x++)
        {
            for (int z = -interactDistance; z <= interactDistance; z++)
            {
                GridPosition testGridPosition = unitGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if ((door == null)||(door.DoorIsOpen()))
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractComplete);
        ActionStart(onActionComplete);

    }
    private void OnInteractComplete()
    {
        ActionComplete();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }
    }
 
}