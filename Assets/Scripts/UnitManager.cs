using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    private List<Unit> unitList;
    private List<Unit> playerUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Another instance exist for unit system:" + transform + "  -  " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        playerUnitList = new List<Unit>();

        enemyUnitList = new List<Unit>();

    }
    // Start is called before the first frame update
    void Start()
    {
        Unit.onUnitSpawn += Unit_OnUnitSpawn;
        Unit.onUnitDeath += Unit_OnUnitDeath;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Unit_OnUnitSpawn(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            playerUnitList.Add(unit);
        }
        unitList.Add(unit);
    }
    private void Unit_OnUnitDeath(object sender, EventArgs e)
    {

        Unit unit = sender as Unit;

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            playerUnitList.Remove(unit);
        }
        unitList.Remove(unit);
    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetPlayerUnitList()
    {
        return playerUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

}
