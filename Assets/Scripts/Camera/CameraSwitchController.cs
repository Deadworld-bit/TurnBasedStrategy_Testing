using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchController : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] LayerMask normalCameraCullingMask;
    [SerializeField] LayerMask actionCameraCullingMask;
    
    private void Start()
    {
        UnitActionBase.OnAnyActionStarted += UnitActionBase_OnAnyActionStarted;
        UnitActionBase.OnAnyActionCompleted += UnitActionBase_OnAnyActionCompleted;
        
        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        //Camera.main.cullingMask = actionCameraCullingMask;
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        //Camera.main.cullingMask = normalCameraCullingMask;
        actionCameraGameObject.SetActive(false);
    }

    private void UnitActionBase_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootArrowAction shootArrowAction:
                float shoulderOffsetAmount = 0.5f;
                Unit shooterUnit = shootArrowAction.GetUnit();
                Unit targetUnit = shootArrowAction.GetTargetUnit();
                
                //Camera position
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;
                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                
                ShowActionCamera();
                break;
        }
    }
    
    private void UnitActionBase_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootArrowAction :
                HideActionCamera();
                break;
        }
    }
}
