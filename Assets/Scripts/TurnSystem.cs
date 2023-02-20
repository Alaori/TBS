using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    private bool isPlayerTurn = true;
    public EventHandler<bool> onTurnChange;
    public static TurnSystem Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Another instance exist for turn system:" + transform + "  -  " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void NextTurn()
    {

        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        if (onTurnChange != null)
        {
            onTurnChange(this, isPlayerTurn);
        }
    }
    public int GetTurnNumber()
    {
        return turnNumber;
    }
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
