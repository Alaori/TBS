using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class UnitUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        Unit.onActionPointsChange += Unit_OnActionPointsChange;
        UpdateActionPointText();
        UpdateHealthBar();
        healthSystem.onDamage += HealthSystem_OnDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateActionPointText()
    {
        actionPointText.text = unit.GetActionPoints().ToString();

    }
    private void Unit_OnActionPointsChange(object sender, EventArgs e)
    {
        actionPointText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthPoints();

    }
    private void HealthSystem_OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
