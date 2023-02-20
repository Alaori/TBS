using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenShakeAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShootAction.onAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.onGrenadeExplode += GrenadeProjectile_OnGrenadeExplode;
        SwordAction.onSwordHit += SwordAction_OnSwordHit;
    }

    private void SwordAction_OnSwordHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(3f);

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(1f);

    }
    private void GrenadeProjectile_OnGrenadeExplode(object sender,EventArgs e)
    {

        ScreenShake.Instance.Shake(5f);

    }
}
