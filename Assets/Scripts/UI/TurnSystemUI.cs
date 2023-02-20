using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnTextObject;
    // Start is called before the first frame update
    void Start()
    {
        enemyTurnTextObject.SetActive(false);
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();

        });
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
        UpdateTurnText();
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void UpdateTurnText()
    {
        turnNumberText.text = "Turn : " + TurnSystem.Instance.GetTurnNumber();

    }
    private void TurnSystem_OnTurnChange(object sender, bool isPlayerTurn)
    {
        UpdateTurnText();
        UpdateEndTurnButton(isPlayerTurn);
        UpdateEnemyTurnText(isPlayerTurn);

    }
    private void UpdateEndTurnButton(bool isPlayerTurn)
    {
        endTurnButton.gameObject.SetActive(isPlayerTurn);
    }
    private void UpdateEnemyTurnText(bool isPlayerTurn)
    {
        enemyTurnTextObject.SetActive(!isPlayerTurn);


    }

}
