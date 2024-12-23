using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    private void Start()
    {
        ShootArrowAction.OnAnyShoot += ShootArrowAction_OnAnyShoot;
    }

    private void ShootArrowAction_OnAnyShoot(object sender, ShootArrowAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
