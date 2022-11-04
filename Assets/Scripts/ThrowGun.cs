using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGun : SpawnGun
{
    public GrabController mainHand;
    public int throwForce;
    public int throwTorque;

    private void Start()
    {
        mainHand = GetComponent<GrabController>();
    }

    public void Throw()
    {
        referenceGun = mainHand.Unequip();

        var droppedGunRigidbody = SpawnGunDummy(referenceGun.transform, true).GetComponent<Rigidbody>();

        droppedGunRigidbody.AddForce(referenceGun.transform.forward * throwForce, ForceMode.Impulse);
        droppedGunRigidbody.AddTorque(referenceGun.transform.right * throwTorque, ForceMode.Acceleration);
    }
}
