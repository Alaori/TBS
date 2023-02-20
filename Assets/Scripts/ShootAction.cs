using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : UnitAction
{
    public static event EventHandler<OnShootEventArgs> onAnyShoot;

    public event EventHandler<OnShootEventArgs> onShoot;
    [SerializeField] private LayerMask obstaclesLayerMask;
    public class OnShootEventArgs : EventArgs 
    {
        public Unit targetUnit;
        public Unit shootingUnit;

    }
    private enum State
    { 
        Aiming,
        Shooting,
        Cooloff,
    }
    private float stateTimer;
    private State state;
    private int maxShootRange = 7;
    private Unit targetUnit;
    private bool canShoot;
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
            case State.Aiming:
                if (targetUnit != null)
                {
                    Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                } break;
            case State.Shooting:
                if ((canShoot)&& (targetUnit != null))
                {
                    Shoot();
                    canShoot = false;
                }
                break;
            case State.Cooloff:
                break;
        }
        if(stateTimer <=0f)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTimer = 0.1f;
                    stateTimer = shootingStateTimer;
                    break;
            case State.Shooting:
                    state = State.Cooloff;
                    float CooloffStateTimer = 0.5f;
                    stateTimer = CooloffStateTimer;
                    break;
            case State.Cooloff:
                    ActionComplete();
                    break;
        } 
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
       // GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxShootRange; x <= maxShootRange; x++)
        {
            for (int z = -maxShootRange; z <= maxShootRange; z++)
            {
               // GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance> maxShootRange)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }
                float unitShouldHeight = 1.7f;
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShouldHeight, shootDirection, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstaclesLayerMask))
                {
                    continue;
                }
                Debug.DrawRay(unitWorldPosition + Vector3.up * unitShouldHeight, transform.forward*2f, Color.white, 100f);
                validGridPositionList.Add(testGridPosition);

            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
       
        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShoot = true;
        ActionStart(onActionComplete);
    }
    private void Shoot()
    {
        if (onShoot != null) 
        {
            onShoot(this, new OnShootEventArgs { targetUnit = targetUnit, shootingUnit = unit});

        }
        if (onAnyShoot != null)
        {
            onAnyShoot(this, new OnShootEventArgs { targetUnit = targetUnit, shootingUnit = unit });

        }
        targetUnit.Damage(40);

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

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
 
    public int GetMaxShootRange()
    {
        return maxShootRange;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1- targetUnit.GetHealthPoints()) *100f),

        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionList(gridPosition).Count;
    }
}

