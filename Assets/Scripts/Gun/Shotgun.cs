using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    #region Variables
    [Header("Necessary Components")]
    public GameObject bulletHole;

    [Header("Ammo Stats")]
    public int projectileCount;
    public float spreadVer;
    public float spreadHor;
    public float rangeEffective;
    public float rangeMax;
    public int damage;
    #endregion

    public override void Ammunition()
    {
        Camera fpsCam = Camera.main;

        for (int i = 0; i < projectileCount; i++)
        {
            var spread = fpsCam.transform.forward;
            spread += fpsCam.transform.TransformDirection(new Vector3(Random.Range(-spreadHor, spreadHor), Random.Range(-spreadVer, spreadVer), 0));

            if (Physics.Raycast(fpsCam.transform.position, spread, out var hit, rangeMax))
            {
                Instantiate(bulletHole, hit.point + (hit.normal * .01f), Quaternion.FromToRotation(Vector3.up, hit.normal));

                if (hit.collider.GetComponent<IDamagable>() != null)
                {
                    hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
                }
            }
        }
    }
}
