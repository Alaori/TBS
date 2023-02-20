using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ragdollRootBone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup(Transform originalRootBone)
    {
        MatchAllTransform(originalRootBone, ragdollRootBone);
        Vector3 randomDirection = new Vector3(Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f));
        AddExplosionOnRagdoll(ragdollRootBone, 300f, transform.position + randomDirection, 10f);
    }
    private void MatchAllTransform(Transform root, Transform clone)
    { 
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllTransform(child, cloneChild);
            }    
        }
    }
    private void AddExplosionOnRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    { 
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                AddExplosionOnRagdoll(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
