using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectedUnitVisual : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    private MeshRenderer meshRenderer;
    private HealthSystem healthSystem;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UnitSystem.Instance.onUnitChange += UnitSystem_OnUnitChange;
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
        VisualUpdate();
        healthSystem = selectedUnit.GetComponent<HealthSystem>();
        healthSystem.onDeath += HealthSystem_OnDeath;

    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        meshRenderer.enabled = false;
    }

    private void TurnSystem_OnTurnChange(object sender, bool isPlayerTurn)
    {
        if(!isPlayerTurn)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UnitSystem_OnUnitChange(object sender, EventArgs empty)
    {
        VisualUpdate();
    }

    private void VisualUpdate()
    {
        if (UnitSystem.Instance.GetSelectedUnit() == selectedUnit)
        {
            meshRenderer.enabled = true;    
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
    private void OnDestroy()
    {
        UnitSystem.Instance.onUnitChange -= UnitSystem_OnUnitChange;

    }

}
