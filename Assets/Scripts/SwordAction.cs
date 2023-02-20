using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : UnitAction
{
    public static event EventHandler onSwordHit;
    public event EventHandler onSwordStart;
    public event EventHandler onSwordEnd;
    private enum State
    {
        swingingBeforeHit,
        swingingAfterHit,

    }
    private State state;
    private float stateTimer;
    private int swordDistance = 1;
    private Unit targetUnit;
    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }

    public override List<GridPosition> GetValidGridPositionList()
{
    GridPosition unitGridPosition = unit.GetGridPosition();
    List<GridPosition> validGridPositionList = new List<GridPosition>();
    for (int x = -swordDistance; x <= swordDistance; x++)
    {
        for (int z = -swordDistance; z <= swordDistance; z++)
        {
            GridPosition testGridPosition = unitGridPosition + new GridPosition(x, z);
            if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
                continue;
            }
            if (!LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
            {
                continue;
            }
            Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(testGridPosition);
            if (targetUnit.IsEnemy() == unit.IsEnemy())
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
        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        state = State.swingingBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;
        if(onSwordStart!=null)
        {
            onSwordStart(this, EventArgs.Empty);
        }
        ActionStart(onActionComplete);
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

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.swingingBeforeHit:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.swingingAfterHit:
               
                break;
     
        }
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    public int GetSwordDistance()
    {
        return swordDistance;
    }
    private void NextState()
    {
        switch (state)
        {
            case State.swingingBeforeHit:
                state = State.swingingAfterHit;
                float afterHitStateTime = 0.1f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);
                if(onSwordHit !=null)
                {
                    onSwordHit(this, EventArgs.Empty);
                }
                break;
            case State.swingingAfterHit:
                if(onSwordEnd !=null)
                {
                    onSwordEnd(this, EventArgs.Empty);
                }
                ActionComplete();
                break;
        }
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
