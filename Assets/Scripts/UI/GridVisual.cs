using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private GridPosition gridVisualPosition;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    public void ShowVisual(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;

    }
    public void HideVisual()
    {
        meshRenderer.enabled = false;
    }
   
    
    public void SetGridPosition(int x, int z)
    {
        this.gridVisualPosition = new GridPosition(x, z);

    }
    public GridPosition GetVisualPosition()
    {
        return this.gridVisualPosition;
    }
}
