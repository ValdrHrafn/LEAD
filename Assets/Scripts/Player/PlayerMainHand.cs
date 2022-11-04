using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainHand : GrabController
{
    public ThrowGun throwGun;

    public void Start()
    {
        throwGun = GetComponent<ThrowGun>();
    }

    public virtual void TriggerAction(InputAction.CallbackContext context)
    {
        if (equippedGun == null) return;

        if (context.performed) equippedGunScript.TriggerPress();
        if (context.canceled) equippedGunScript.TriggerRelease();
    }

    public void ThrowAction(InputAction.CallbackContext context)
    {
        if (equippedGun == null) return;

        if (context.performed) throwGun.Throw();
    }
}
