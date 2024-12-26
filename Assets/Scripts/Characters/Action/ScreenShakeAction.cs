using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    private void Start()
    {
        ShootArrowAction.OnAnyShoot += ShootArrowAction_OnAnyShoot;
        SpellBurstProjectile.OnAnySpellBurst += SpellBurstProjectile_OnAnySpellBurst;
    }

    private void ShootArrowAction_OnAnyShoot(object sender, ShootArrowAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }

    private void SpellBurstProjectile_OnAnySpellBurst(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }
}
