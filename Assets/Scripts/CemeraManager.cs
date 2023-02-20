using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CemeraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera;
    // Start is called before the first frame update
    void Start()
    {
        UnitAction.onActionStart += UnitAction_OnActionStart;
        UnitAction.onActionEnd+= UnitAction_OnActionEnd;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }
    private void UnitAction_OnActionStart(object sender, EventArgs e)
    {
        switch(sender)
            {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootingDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * (shootingDirection *0.5f);
                Vector3 actionCameraPosition = (shooterUnit.GetWorldPosition() + cameraCharacterHeight + (shoulderOffset + shootingDirection * -1));
                actionCamera.transform.position = actionCameraPosition;
                actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
            }
    }
    private void UnitAction_OnActionEnd(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction:
               HideActionCamera();
                break;
        }
    }
}
