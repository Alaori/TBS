using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    private Transform cameraTransform;
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 cameraDirection = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + cameraDirection *-1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
