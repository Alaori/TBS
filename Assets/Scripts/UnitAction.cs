using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class UnitAction : MonoBehaviour
{
    public static event EventHandler onActionStart;
    public static event EventHandler onActionEnd;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public Unit GetUnit()
    {
        return unit;
    }
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidGridPositionList();

    public virtual int GetActionPointCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        if (onActionStart != null)
        {
            onActionStart(this, EventArgs.Empty);
        }
    }
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        if (onActionEnd != null)
        {
            onActionEnd(this, EventArgs.Empty);
        }
    }

    public EnemyAIAction SelectEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidGridPositionList();
        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }
 
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else 
        {

            return null;
        }
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    
}
