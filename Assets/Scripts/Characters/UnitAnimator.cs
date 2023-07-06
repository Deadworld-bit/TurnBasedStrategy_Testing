using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        //try to get move action not to get move action to prevent the game from breaking if there are no move action
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        
        //Get Shoot action
        if (TryGetComponent<ShootArrowAction>(out ShootArrowAction shootArrowAction))
        {
            shootArrowAction.OnShoot += ShootArrowAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", true);
    }
    
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", false);
    }
    
    private void ShootArrowAction_OnShoot(object sender, EventArgs e)
    {
        animator.SetTrigger("isShooting");
    }
}
