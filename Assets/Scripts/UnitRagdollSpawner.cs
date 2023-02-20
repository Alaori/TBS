using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UnitRagdollSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDeath += HealthSystem_OnDeath;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
       Transform ragdollTransform =  Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
