using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    private List<UnitActionButtonUI> unitActionButtonUIList;
    [SerializeField] private TextMeshProUGUI actionPointsText; 

    private void Awake()
    {
        unitActionButtonUIList = new List<UnitActionButtonUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Unit.onActionPointsChange += Unit_OnActionPointsChange;
        UnitSystem.Instance.onUnitChange += UnitSystem_OnUnitChange;
        UnitSystem.Instance.onActionChange += UnitSystem_OnActionChange;
        UnitSystem.Instance.onActionStart += UnitSystem_OnActionStart;
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
        CreateActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreateActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainer)
        {

            Destroy(buttonTransform.gameObject);
        }
        unitActionButtonUIList.Clear();
        Unit selectedUnit = UnitSystem.Instance.GetSelectedUnit();
        foreach (UnitAction unitAction in selectedUnit.GetUnitActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            UnitActionButtonUI unitActionButtonUI = actionButtonTransform.GetComponent<UnitActionButtonUI>();
            unitActionButtonUI.SetUnitAction(unitAction);
            unitActionButtonUIList.Add(unitActionButtonUI);
        }
    }
    private void UnitSystem_OnUnitChange(object snder, EventArgs e)
    {
        UpdateActionPoints();

        CreateActionButtons();
        UpdateSelectedVisual();
    }
    private void UpdateSelectedVisual()
    {
        foreach (UnitActionButtonUI unitActionButtonUI in unitActionButtonUIList)
        {
            unitActionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UnitSystem_OnActionChange(object snder, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitSystem.Instance.GetSelectedUnit();
        actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();

    }

    private void UnitSystem_OnActionStart(object snder, EventArgs e)
    {
        UpdateActionPoints();  
    }
    private void TurnSystem_OnTurnChange(object snder, bool isPlayerTurn)
    {
        UpdateActionPoints();
    }
    private void Unit_OnActionPointsChange(object snder, EventArgs e)
    {
        UpdateActionPoints();
    }
}
