using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DummyGun : MonoBehaviour
{
    //Accessible for other scripts
    public GameObject referencedGun;

    public bool isHarmful;

    //Gameplay Variables
    [SerializeField] private float grabTime;
    [SerializeField] private int throwDamage;

    //Processing Variables
    private Coroutine enableGrabCoroutine;
    private Material gunMat;
    private SphereCollider grabTrigger;

    public void Initialize(GameObject gunReference, bool isThrown)
    {
        //Setting necessary variables
        isHarmful = isThrown;
        GetComponent<Rigidbody>().maxAngularVelocity = 100;

        //Looks
        referencedGun = gunReference;
        var gunReferenceCollider = gunReference.GetComponent<BoxCollider>();
        var droppedGunCollider = gameObject.AddComponent<BoxCollider>();
        droppedGunCollider.isTrigger = false;
        droppedGunCollider.center = gunReferenceCollider.center;
        droppedGunCollider.size = gunReferenceCollider.size;

        //Instantiate model and get Material to use as indicator 
        gunMat = Instantiate(gunReference.transform.GetChild(0), transform).GetComponentInChildren<Renderer>().material;

        //Grab Collider
        grabTrigger = GetComponentInChildren<SphereCollider>();
        enableGrabCoroutine = StartCoroutine(EnableGrabInTime(grabTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collider = collision.collider;
        if (collider.tag == "Player") return;

        StopCoroutine(enableGrabCoroutine);
        EnableGrab();
        gunMat.color = Color.red;

        if (!isHarmful) return;
        var damagableCollider = collider.GetComponent<IDamagable>();
        if (damagableCollider != null)
        {
            damagableCollider.TakeDamage(throwDamage);
        }
    }

    IEnumerator EnableGrabInTime(float enableGrabTime)
    {
        DisableGrab();

        float i = 0;
        float refresh = .1f;
        while (i < enableGrabTime)
        {
            i += refresh;
            yield return new WaitForSeconds(.1f);
        }

        EnableGrab();
    }

    void DisableGrab()
    {
        grabTrigger.enabled = false;
        gunMat.color = Color.yellow;
    }

    void EnableGrab()
    {
        grabTrigger.enabled = true;
        gunMat.color = Color.green;
    }
}