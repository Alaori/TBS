using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Unit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fullCoverSprite;
    [SerializeField] private SpriteRenderer halfCoverSprite;
    public static event EventHandler onUnitSpawn;
    public static event EventHandler onUnitDeath;

    [SerializeField] private bool isEnemy;
    public static event EventHandler onActionPointsChange;
    private const int totalActionPoints = 2;

    private GridPosition gridPosition;
    private UnitAction[] unitActionArray;
    private HealthSystem healthSystem;
    private int actionPoints = totalActionPoints;

    private CoverType coverType;
    private void Awake()
    {       
        unitActionArray = GetComponents<UnitAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        coverType = CoverType.None;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(gridPosition,this);
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
        healthSystem.onDeath += HealthSystem_OnDeath;
        if(onUnitSpawn !=null)
        {
            onUnitSpawn(this, EventArgs.Empty);
        }
    }


    // Update is called once per frame
    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoved(this, oldGridPosition, newGridPosition);

        }
    }
    public T GetAction<T>()where T: UnitAction 
    {
        foreach (UnitAction unitAction in unitActionArray)
        {
            if (unitAction is T)
            {
                return (T)unitAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
  
    public UnitAction[] GetUnitActionArray()
    {
        return unitActionArray;

    }
    public bool CanSpendActionPoints(UnitAction unitAction)
    {
        if (actionPoints >= unitAction.GetActionPointCost())
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    private void SpendActionPoints(int amount)
    {

        actionPoints -= amount;

        if (onActionPointsChange != null)
        {
            onActionPointsChange(this, EventArgs.Empty);
        }

    }
    public bool TrySpendActionPoints(UnitAction unitAction)
    {
   
        if (CanSpendActionPoints(unitAction))
        {
            SpendActionPoints(unitAction.GetActionPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetActionPoints()
    {
        return actionPoints;

    }
    private void TurnSystem_OnTurnChange(object sender, bool isPlayerTurn)
    {
        if ((IsEnemy() && !isPlayerTurn) || (!IsEnemy() && isPlayerTurn))
        {
            actionPoints = totalActionPoints;

            if (onActionPointsChange != null)
            {
                onActionPointsChange(this, EventArgs.Empty);
            }
        }
    } 
    public bool IsEnemy()
    {
        return isEnemy;
    }
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtPosition(gridPosition, this);
        Destroy(gameObject);
        if (onUnitDeath != null)
        {
            onUnitDeath(this, EventArgs.Empty);
        }
    }
    public float GetHealthPoints()
    {
        return healthSystem.GetHealthPoints();

    }
  
    public void SetCoverType(CoverType type)
    {
        coverType = type;
        UpdateCoverType();
    }

    private void UpdateCoverType()
    {
        switch(coverType)
        {
            case CoverType.Full:
                fullCoverSprite.enabled = true;
                halfCoverSprite.enabled = false;
                break;
            case CoverType.Half:
                fullCoverSprite.enabled = false;
                halfCoverSprite.enabled = true;
                break;
            case CoverType.None:
                fullCoverSprite.enabled = false;
                halfCoverSprite.enabled = false;
                break;

        }
    }
}
