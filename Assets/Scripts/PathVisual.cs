/* Need refactor code to make neat
 * 
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathVisual : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private LayerMask gridLayerMask;
    private CoverVisual coverVisual;
    [SerializeField] private GameObject gameObject;
    private bool validPosition;
    [SerializeField] private MousePointer mousePointer;
    [SerializeField] private LineRenderer lineRenderer;
    private List<GridPosition> movePathList;
    
    private void Start()
    {
        Vector3 p = new Vector3(1, 1, 1);

        UnitSystem.Instance.onUnitChange += UnitSystem_OnUnitChange;
        TurnSystem.Instance.onTurnChange += TurnSystem_OnTurnChange;
        mousePointer.onMousePositionChange += MousePointer_OnMousePositionChange;
        lineRenderer.enabled = false;
    }

    private void TurnSystem_OnTurnChange(object sender, bool isPlayerTurn)
    {
        if(!isPlayerTurn)
        {
            lineRenderer.enabled = false;
        }
    }

    private void UnitSystem_OnUnitChange(object sender, EventArgs e)
    {
        lineRenderer.enabled = false;
    }

    private void MousePointer_OnMousePositionChange(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        if(coverVisual!=null)
        {
            coverVisual.DisableSpriteRenderer();
        }
        validPosition = false;
        Unit selectedUnit = UnitSystem.Instance.GetSelectedUnit();
        UnitAction unitAction = UnitSystem.Instance.GetSelectedAction();
        if (selectedUnit != null)
        {
            MoveAction moveAction = selectedUnit.GetComponent<MoveAction>();
        
        if (unitAction != moveAction)
        {
            lineRenderer.enabled = false;
            return;
        }
        else
        {
            Vector3 mousePosition = mousePointer.GetPosition();
            if ((mousePosition.x >= 0) && (mousePosition.z >= 0))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(mousePointer.GetPosition());
                GridPosition selectedUnitGridPosition = selectedUnit.GetGridPosition();
                List<GridPosition> testGridPositionList = moveAction.GetValidGridPositionList();
                foreach (GridPosition gridPosition in testGridPositionList)
                {
                    if (gridPosition == mouseGridPosition)
                    {
                        validPosition = true;
                      
                    }
                }

                    if ((validPosition == true) && (mouseGridPosition != selectedUnitGridPosition))
                    {
                        movePathList = PathFinding.Instance.FindPath(selectedUnit.GetGridPosition(), mouseGridPosition, out int pathLength);
                        Vector3[] points = new Vector3[movePathList.Count];
                        for (int i = 0; i < movePathList.Count; i++)
                        {
                            Vector3 point = LevelGrid.Instance.GetWorldPosition(movePathList[i]);
                            point.y += 0.3f;
                            points[i] = point;
                        }
                        lineRenderer.SetVertexCount(points.Length);
                        for (int a = 0; a < points.Length; a++)
                        {
                            lineRenderer.SetPosition(a, points[a]);
                        }
                        lineRenderer.SetPositions(points);
                        lineRenderer.enabled = true;
                        //north Check
                        Vector3 startVector = new Vector3(mousePosition.x, .7f, mousePosition.z);
                        Vector3 northVector = new Vector3(0, 0, 3);
                        Vector3 downVector = new Vector3(0, 10, 3);
                        RaycastHit hit;
                        float distance = 3f;
                        if (Physics.Raycast(startVector, transform.forward, out hit, distance, obstaclesLayerMask))
                        {
                            if (hit.collider.gameObject.GetComponent<CoverObject>() != null)
                            {
                              
                                CoverObject coverObject = hit.collider.gameObject.GetComponent<CoverObject>();
                                CoverType type = coverObject.GetCoverType();
                                Debug.Log(hit.collider.gameObject);
                            
                            if (Physics.Raycast(startVector, -transform.up, out hit, distance, gridLayerMask))
                            {
                                Debug.Log("hi");
                                Debug.Log(hit.collider.gameObject);
                                coverVisual = hit.collider.gameObject.GetComponentInParent<CoverVisual>();
                                coverVisual.EnableSpriteRenderer();
                                coverVisual.SetSprite(type, coverVisual.GetSpriteRenderer("North"));
                                //selectedUnit.SetCoverType(type);
                            }
                            }
                        }
                    }
                    else
                    {
                        lineRenderer.enabled = false;
                    }
            }
            else
            {
                lineRenderer.enabled = false;

            }
        }
        }
    }
}