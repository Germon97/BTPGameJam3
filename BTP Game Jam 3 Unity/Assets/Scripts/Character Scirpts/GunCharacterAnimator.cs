using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCharacterAnimator : CharacterAnimator
{
    [SerializeField]
    protected Transform gunTransform;

    public void UpdateGunRotatation(Vector3 aimAt)
    {
        Vector3 dir = aimAt - gunTransform.position;
        //gunTransform.localRotation = Quaternion.FromToRotation(gunTransform.position, aimAt);
        // gunTransform.rotation = Quaternion.LookRotation(aimAt - gunTransform.position);

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle += ((transform.position.x - aimAt.x > 0) ? 180 : 0);

        gunTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
