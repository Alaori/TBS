using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class UnitSystem : MonoBehaviour
{
    public static UnitSystem Instance { get; private set; }
    private UnitAction selectedAction;
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    public event EventHandler onUnitChange;
    public event EventHandler onActionChange;
    public event EventHandler<bool> onBusyChange;
    public event EventHandler onActionStart;

    private bool isBusy;
    // Start is called before the first frame update
    void Start()
    {
        SetUnit(selectedUnit);
    }
    private void Awake()
    {
        if(Instance !=null)
        {
            Debug.LogError("Another instance exist for unit system:" + transform + "  -  " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(UnitSelection())
        {
            return;
        }
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        SelectAction();
       


    }
    private void SelectAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePointer.GetMousePoition());
       
            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedUnit.TrySpendActionPoints(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ResetBusy);
                   
                    if (onActionStart != null)
                    {
                        onActionStart(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
    private bool UnitSelection()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, unitLayerMask))
            {
                if (rayCastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
                    if (unit.IsEnemy())
                    {
                        return false;
                    }
                    SetUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }
    private void SetUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        if (onUnitChange != null)
        {
            onUnitChange(this, EventArgs.Empty);
        } 
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    private void SetBusy()
    {
        if (onBusyChange != null)
        {
            onBusyChange(this, isBusy);
        }
        
        isBusy = true;    
    }
    private void ResetBusy()
    {
        if (onBusyChange != null)
        {
            onBusyChange(this, isBusy);
        }
        isBusy = false;
    }
    public void SetSelectedAction(UnitAction unitAction)
    {
        selectedAction = unitAction;
        
        if (onActionChange != null)
        {
            onActionChange(this, EventArgs.Empty);
        }

    }
    public UnitAction GetSelectedAction()
    {
        return selectedAction;
    }
}
