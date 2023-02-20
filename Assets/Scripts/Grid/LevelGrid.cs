using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    private GridSystem<GridObject> gridSystem;
    [SerializeField] private Transform db;
    public event EventHandler onUnitMoveGridPosition;

    [SerializeField] private int length;
    [SerializeField] private int width;
    [SerializeField] private float cellSize;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than 1 instance");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem<GridObject>(width, length, cellSize, (GridSystem<GridObject> gameObject, GridPosition gridPosition) => new GridObject(gameObject, gridPosition));
        //    gridSystem.cDebug(db);
    }
    private void Start()
    {
        PathFinding.Instance.Setup(width, length, cellSize);
    }
    public void AddUnitAtPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.AddUnit(unit);
    }
    public List<Unit> GetUnitAtPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetUnitList();
    }
    public void RemoveUnitAtPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        gridObj.RemoveUnit(unit);
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return gridSystem.GetPosition(gridPosition);
    }
    public void UnitMoved(Unit unit, GridPosition startPosition, GridPosition currentPosition)
    {
        RemoveUnitAtPosition(startPosition, unit);
        AddUnitAtPosition(currentPosition, unit);
        if (onUnitMoveGridPosition != null)
        {
            onUnitMoveGridPosition(this, EventArgs.Empty);

        }
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridSystem.IsValidGridPosition(gridPosition);
    }
    public bool HasUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.HasAUnit();
    }

    public int GetWidth()
    {
        return gridSystem.GetWidth();
    }
    public int GetLength()
    {
        return gridSystem.GetLength();
    }
    public Unit GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObj = gridSystem.GetGridObject(gridPosition);
        return gridObj.GetUnit();
    }

    public Door GetDoorAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetDoor();

    }
    public void SetDoorAtGridPosition(GridPosition gridPosition, Door door)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetDoor(door);

    } 
 
}
