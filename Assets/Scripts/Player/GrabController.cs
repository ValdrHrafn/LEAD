using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    public GameObject equippedGun;
    public GunBase equippedGunScript;

    public Transform handConnection;
    public Transform gunStorage;

    private void OnTriggerEnter(Collider collider)
    {
        if (equippedGun != null) return;
        if (collider.gameObject.CompareTag("Gun")) EquipGun(collider);
    }

    private void EquipGun(Collider grabCollider)
    {
        equippedGun = grabCollider.transform.parent.gameObject.GetComponent<DummyGun>().referencedGun;
        Destroy(grabCollider.transform.parent.gameObject);

        equippedGunScript = equippedGun.GetComponent<GunBase>();

        Place.UnderParent(equippedGun, handConnection);

        equippedGun.SetActive(true);
        equippedGunScript.Chamber();
    }

    public GameObject Unequip()
    {
        if(equippedGun != null)
        {
            equippedGun.SetActive(false);
            equippedGun.transform.SetParent(gunStorage);
            var gunTransform = equippedGun;

            equippedGun = null;
            return gunTransform;
        }
        return null;
    }
}