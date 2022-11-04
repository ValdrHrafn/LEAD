using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGun : SpawnGun
{
    public Transform handPosition;

    public int dropForce;
    public int dropTorque;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) PlayerDrop(handPosition);
    }

    public void PlayerDrop(Transform heldPosition)
    {
        var droppedGunRigidbody = SpawnGunDummy(heldPosition, false).GetComponent<Rigidbody>();

        droppedGunRigidbody.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        droppedGunRigidbody.AddTorque(Random.onUnitSphere * dropTorque, ForceMode.Acceleration);
    }

    public void Drop(Transform heldPosition)
    {
        var droppedGunRigidbody = SpawnGunDummy(heldPosition, false).GetComponent<Rigidbody>();

        droppedGunRigidbody.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        droppedGunRigidbody.AddTorque(Random.onUnitSphere * dropTorque, ForceMode.Acceleration);
    }
}