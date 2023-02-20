using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform swordTransform;

    [SerializeField] private Transform rifleTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.onStartMoving += MoveAction_OnStartMoving;
            moveAction.onStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.onShoot += ShootAction_onShoot;
        }
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.onSwordEnd += SwordAction_OnSwordEnd;
            swordAction.onSwordStart += SwordAction_OnSwordStart;

        }
    }

    private void SwordAction_OnSwordStart(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("swordSlash");
    }

    private void SwordAction_OnSwordEnd(object sender, EventArgs e)
    {
        EquipRifle();
    }

    // Start is called before the first frame update
    void Start()
    {
        EquipRifle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", true);
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", false);
    }
    private void ShootAction_onShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        if (e.targetUnit != null)
        {
            animator.SetTrigger("shoot");
            Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
            BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
            Vector3 targetUnitShootPosition = e.targetUnit.GetWorldPosition();
            targetUnitShootPosition.y = shootPointTransform.position.y;
            bulletProjectile.Setup(targetUnitShootPosition);
        }
    }
    private void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }
    private void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);


    }
}
