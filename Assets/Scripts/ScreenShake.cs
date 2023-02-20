using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("instance exist");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void Shake(float intensity)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
} 
