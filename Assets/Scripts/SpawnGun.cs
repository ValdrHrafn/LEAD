using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGun : MonoBehaviour
{
    public GameObject dummyGunPrefab;
    public GameObject referenceGun;

    public GameObject SpawnGunDummy(Transform dummyTransform, bool isThrown)
    {
        var dummyGun = Instantiate(dummyGunPrefab, dummyTransform.position, dummyTransform.rotation);


        var dummyGunScript = dummyGun.GetComponent<DummyGun>();
        dummyGunScript.Initialize(referenceGun, isThrown);

        return dummyGun;
    }

}
