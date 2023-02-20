using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBusyUI : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        UnitSystem.Instance.onBusyChange += UnitSystem_OnBusyChange;
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
        Show();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void UnitSystem_OnBusyChange(object sender, bool isBusy)
    {
        if (isBusy)
        {
            Show();
        }
        else 
        {
            Hide();
        }
    }
    private void TurnSystem_OnTurnChange(object sender, bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
