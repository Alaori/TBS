using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : UnitAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
        [SerializeField] private LayerMask obstaclesLayerMask;

    int maxThrowRange = 10;
    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
      //  int targetCount = unit.GetAction<GrenadeAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
             actionValue = 1000,
         };
       
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        // GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxThrowRange; x <= maxThrowRange; x++)
        {
            for (int z = -maxThrowRange; z <= maxThrowRange; z++)
            {
                // GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowRange)
                {
                    continue;
                }
                if (!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition))
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
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeComplete);
        ActionStart(onActionComplete);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }
    }
    private void OnGrenadeComplete()
    {
        ActionComplete();
    }
    public override int GetActionPointCost()
    {
        if (unit.GetActionPoints() != 0)
        {
            return unit.GetActionPoints();
        }
        else
        {
            return 1;
        }
    }
}
