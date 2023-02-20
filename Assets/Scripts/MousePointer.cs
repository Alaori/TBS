using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MousePointer : MonoBehaviour
{
    public static MousePointer Instance { get; private set; }
    public event EventHandler onMousePositionChange;
 
    [SerializeField] private LayerMask mousePointerLayerMask;
    [SerializeField] private LayerMask TmousePointerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
  
        if (transform.position != GetMousePoition())
        {
            if(onMousePositionChange !=null)
            {
                onMousePositionChange(this, EventArgs.Empty);
            }
            transform.position = GetMousePoition();
        }  
    }

    private void Awake()
    {
        Instance = this;
    }
    public static Vector3 GetMousePoition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, Instance.mousePointerLayerMask);
    //    Debug.Log(rayCastHit.point.normalized) ;
        return rayCastHit.point;
    }
    public Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, Instance.mousePointerLayerMask);
        //    Debug.Log(rayCastHit.point.normalized) ;
        return rayCastHit.point;

    }

}
