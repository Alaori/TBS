using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridVisualSystem : MonoBehaviour
{
    [Serializable] public struct GridVisualMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        LightRed
    }
    [SerializeField] private List<GridVisualMaterial> GridVisualMaterialList;
    [SerializeField] private GameObject VisualGridContainer;
    public static GridVisualSystem Instance { get; private set; }
    [SerializeField] Transform gridVisualPrefab;
    private GridVisual[,] gridvisualArray;

    public GridVisual GetGridVisual(int x, int z)
    {
        return gridvisualArray[x, z];

    }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
           gridvisualArray = new GridVisual[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetLength()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridVisualPrefab.GetComponent<GridVisual>().SetGridPosition(x,z);
                Transform gridVisualTransform = Instantiate(gridVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridVisualTransform.SetParent(VisualGridContainer.transform);
                gridvisualArray[x, z] = gridVisualTransform.GetComponent<GridVisual>();
            }
        }
        UnitSystem.Instance.onActionChange += UnitSystem_OnActionChange;
        LevelGrid.Instance.onUnitMoveGridPosition += LevelGrid_OnUnitMoveGridPosition;
        UpdateGridVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideAllVisual()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                gridvisualArray[x, z].HideVisual();
            }
        }
    }
    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridvisualArray[gridPosition.x, gridPosition.z].ShowVisual(GetGridVisualTypeMaterial(gridVisualType));
        }
    }
    public void UpdateGridVisual()
    {
        HideAllVisual();
        Unit selectedUnit = UnitSystem.Instance.GetSelectedUnit();
        UnitAction selectedAction = UnitSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType = GridVisualType.White;
        switch(selectedAction)
        {
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootRange(), GridVisualType.LightRed);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeMelee(selectedUnit.GetGridPosition(), swordAction.GetSwordDistance(), GridVisualType.LightRed);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Blue;
                break;
            //   ShowGridPositionRangeMelee(selectedUnit.GetGridPosition(), grenadeAction.getmax(), GridVisualType.LightRed);
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                break;

        }
        ShowGridPositionList(selectedAction.GetValidGridPositionList(),gridVisualType);

    }
    private void UnitSystem_OnActionChange(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    private void LevelGrid_OnUnitMoveGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    { 
        foreach (GridVisualMaterial gridVisualMaterial in GridVisualMaterialList)
        {
            if(gridVisualMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualMaterial.material;

            }
        }

        return null;
    }
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    private void ShowGridPositionRangeMelee(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
               
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
}
