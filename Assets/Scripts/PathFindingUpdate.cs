using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PathFindingUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DestructibleCrate.onCrateDestroy += DestructibleCrate_OnCrateDestroy;
        
    }

    private void DestructibleCrate_OnCrateDestroy(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        PathFinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
