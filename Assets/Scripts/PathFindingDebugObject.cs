using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PathFindingDebugObject : GridDb
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro fCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

    private PathNode pathNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }
    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        if(pathNode.IsWalkable())
        {
            isWalkableSpriteRenderer.color = Color.green;
        }
        else
        {
            isWalkableSpriteRenderer.color = Color.red;
        }

    }
  
}
