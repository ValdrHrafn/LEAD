using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IThrowable
{
    #region Variables
    [Header("Key Bindings")]
    [SerializeField] public KeyCode inputShoot = KeyCode.Mouse0;

    [Header("\nPosition")]

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private int shotsMax;
    [SerializeField] private float reloadTime;
    [SerializeField] private int projectileCount;
    [SerializeField] private float spreadVer;
    [SerializeField] private float spreadHor;
    [SerializeField] private float rangeEffective;
    [SerializeField] private float rangeMax;
    [SerializeField] private int damage;

    [Header("\nOutput")]
    [SerializeField] private int shotsCount;
    [SerializeField] private bool isReloading;
    [SerializeField] private bool isBroken;
    #endregion

    protected virtual void Update()
    {
        if (shotsCount <= 0 && !isReloading)
        {
            isReloading = true;
            Invoke("Reload", reloadTime);
        }
    }

    public virtual void Shoot()
    {
        Camera fpsCam = Camera.main;

        if (health <= 0)
        {
            GunBreak();
            Debug.Log("MISFIRE");
            return;
        }

        if(shotsCount > 0)
        {
            for (int i = 0; i < projectileCount; i++)
            {
                var spread = fpsCam.transform.forward;
                spread += fpsCam.transform.TransformDirection(new Vector3(Random.Range(-spreadHor, spreadHor), Random.Range(-spreadVer, spreadVer), 0));

                if (Physics.Raycast(fpsCam.transform.position, spread, out var hit, rangeMax))
                {
                    if (hit.collider.GetComponent<IDamagable>() != null)
                    {
                        hit.collider.GetComponent<IDamagable>().TakeDamage(damage);
                    }

                    Debug.DrawRay(fpsCam.transform.position, spread, Color.red, 1f);
                }
            }
            shotsCount--;
            health--;
        }
    }

    public virtual void GunBreak() //statemachien
    {
        isBroken = true;
    }

    public virtual void Reload()
    {
        shotsCount = shotsMax;
        isReloading = false;
    }

    public void ThrowObject() {

    }
}