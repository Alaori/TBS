using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : UnitAction
{
    
//      public delegate void SpinCompleteDelegate();
    private float totalSpinAmount;
  //  private SpinCompleteDelegate spinComplete;
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
        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }
    public override void TakeAction(GridPosition gridPosition,Action onActionComplete)
    {
        totalSpinAmount = 0;
        ActionStart(onActionComplete);

    }
    public override string GetActionName()
    {
        return "Spin";
    }
    public override List<GridPosition> GetValidGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>
        {
            unitGridPosition
        };
    }
    public override int GetActionPointCost()
    {
        return 2;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
        actionValue = 0,

        };
    }
}
