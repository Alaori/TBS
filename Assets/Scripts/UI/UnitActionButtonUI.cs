using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;
    private UnitAction unitAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUnitAction(UnitAction unitAction)
    {
        this.unitAction = unitAction;
        textMeshPro.text = unitAction.GetActionName();
        button.onClick.AddListener(() 
        => {
            UnitSystem.Instance.SetSelectedAction(unitAction);

            });
    }
    public void UpdateSelectedVisual()
    {
        UnitAction selectedUnitAction = UnitSystem.Instance.GetSelectedAction();
        if (selectedUnitAction == unitAction)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);

        }
    }
}
