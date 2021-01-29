using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : GunCharacterAnimator
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        Init();
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        LookAt(mousePos);
        UpdateGunRotatation(mousePos);
    }
}
