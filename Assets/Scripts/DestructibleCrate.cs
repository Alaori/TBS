using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DestructibleCrate : MonoBehaviour
{
    [SerializeField] private Transform crateDestroyPrefab;
    public static event EventHandler onCrateDestroy;
    private GridPosition gridPosition;

    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public void Damage()
    {
        Transform crateDestroyTransform = Instantiate(crateDestroyPrefab, transform.position, transform.rotation);
        AddExplosionOnCrate(crateDestroyTransform, 150f, transform.position, 10f);
        Destroy(gameObject);
        if(onCrateDestroy!=null)
        {
            onCrateDestroy(this, EventArgs.Empty);

        }
    }
    private void AddExplosionOnCrate(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            AddExplosionOnCrate(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
