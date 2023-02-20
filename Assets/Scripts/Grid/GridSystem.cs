using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem<TGridObject>
{
    private int width;
    private int length;
    private float cellSize;
    private TGridObject[,] gridArray;
    public GridSystem(int width, int length,float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject>createGridObject)
    {
        this.width = width;
        this.cellSize = cellSize;
        this.length = length;
        gridArray = new TGridObject[width, length];
        for (int x = 0; x < width; x++)
        {
            for(int z =0; z <length;z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridArray[x,z] = createGridObject(this, gridPosition);
                
            }
        }

    }
    public Vector3 GetPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize)
            );
        ;
    }
    public void cDebug(Transform db)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
             Transform dbt =  GameObject.Instantiate(db, GetPosition(gridPosition), Quaternion.identity);
             GridDb gdb = dbt.GetComponent<GridDb>();
                gdb.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridArray[gridPosition.x, gridPosition.z];

    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < width && gridPosition.z < length;

    }
    public int GetWidth()
    {
        return width;
    }
    public int GetLength()
    {
        return length;
    }
}
