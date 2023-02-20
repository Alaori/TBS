using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayerMask;

    public static event EventHandler onGrenadeExplode;
    [SerializeField] private Transform grendadeExplodeEffectPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve grenadeAnimationCurve;
    private Vector3 targetPosition;
    private Action onGrenadeComplete;
    private float totalDistnace;
    private Vector3 positionXZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moreDirection = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moreDirection * moveSpeed * Time.deltaTime;
        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized =1- distance / totalDistnace;
        float maxHeight = totalDistnace/ 3f;
        float positionY = grenadeAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);
        if (Vector3.Distance(positionXZ, targetPosition) < .2f)
        {
            float damageRadius = 6f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach(Collider collider in colliderArray)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, collider.transform.position - transform.position, out hit, damageRadius,obstaclesLayerMask))
                {
                    if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                    {
                        if (Physics.Raycast(targetPosition, targetUnit.GetWorldPosition(), obstaclesLayerMask))
                        {
                            targetUnit.Damage(100);

                        }
                    }
                    if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                    {
                        destructibleCrate.Damage();

                    }
                }

            }
            if(onGrenadeExplode !=null)
            {
                onGrenadeExplode(this, EventArgs.Empty);
            }
            trailRenderer.transform.parent = null;

            Instantiate(grendadeExplodeEffectPrefab, targetPosition + Vector3.up * 1, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeComplete)
    {
        this.onGrenadeComplete = onGrenadeComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistnace = Vector3.Distance(transform.position, targetPosition);
    }

}
