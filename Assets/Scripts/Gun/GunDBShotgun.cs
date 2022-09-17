using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDBShotgun : Gun
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(inputShoot) || Input.GetKeyUp(inputShoot))
        {
            Shoot();
        }
    }
}
