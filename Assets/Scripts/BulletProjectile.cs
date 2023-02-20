using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Instantiate a bullet prefab and control it's movement
 */
public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitEffectPrefab;

    private Vector3 targetPositionVector3;
    public void Setup(Vector3 targetPosition)
    {
        targetPositionVector3 = targetPosition;

    }


    void Update()
    {
        Vector3 moveDirection = (targetPositionVector3 - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPositionVector3);
        float moveSpeed = 50f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        float distanceAfter = Vector3.Distance(transform.position, targetPositionVector3);

        if (distanceToTarget < distanceAfter)
        {
            transform.position = targetPositionVector3;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitEffectPrefab, targetPositionVector3, Quaternion.identity);
        }
    }
}
