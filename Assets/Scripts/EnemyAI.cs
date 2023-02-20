using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyAI : MonoBehaviour
{
   
    private float timer;
    private enum State
    {
        waitingForTurn,
        takingTurn,
        busy
    }
    private State state;

    private void Awake()
    {
        state = State.waitingForTurn;
    }
    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        switch(state)
        {
            case State.waitingForTurn:
                break;
            case State.takingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.busy:
                break;
        }
    }
    private void TurnSystem_OnTurnChange(object sender, bool isPlayerTurn)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.takingTurn;
            timer = 2f;
        }
    }
    private bool TryEnemyAIAction(Action onEnemyAIActionComplete)
    {

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {

            if (TryEnemyAITakeAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.takingTurn;

    }
    private bool TryEnemyAITakeAction (Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction selectedEnemyAIAction = null;
        UnitAction selectUnitAction = null;
     
        foreach (UnitAction unitAction in enemyUnit.GetUnitActionArray())
        {
            
            if(!enemyUnit.CanSpendActionPoints(unitAction))
            {
                continue;
            }
            if (selectedEnemyAIAction == null)
            {
                
                selectedEnemyAIAction = unitAction.SelectEnemyAIAction();
                selectUnitAction = unitAction;
                
            }
            else
            {
                EnemyAIAction testEnemyAIAction = unitAction.SelectEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > selectedEnemyAIAction.actionValue)
                {
                    selectedEnemyAIAction =testEnemyAIAction;
                    selectUnitAction = unitAction;
                  
                }

            }
        }
        if (selectedEnemyAIAction != null && enemyUnit.TrySpendActionPoints(selectUnitAction))
        {
            selectUnitAction.TakeAction(selectedEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
