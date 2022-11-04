using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public abstract class GunBase : MonoBehaviour
{
    #region Variables
    [Header("Necessary Components")]
    public ScriptableObject triggerType;
    public ReloadBar reloadBar;

    [Header("\nGun Loading Stats")]
    public int chamberedShotsMax;
    public float chamberTime;

    [Header("\nIndividual Variables")]
    public int gunHealthMin;
    public int gunHealthMax;
    public int gunHealth;

    [Header("\nProcessing Variables")]
    private int chamberedShots;
    private bool isChambering;
    #endregion

    public virtual void TriggerPress()
    {
        Shoot();
    }

    public virtual void TriggerRelease()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (chamberedShots > 0)
        {
            chamberedShots--;
            gunHealth--;

            Ammunition();

            if (chamberedShots <= 0 && !isChambering)
            {
                Chamber();
            }
        }
        else if (!isChambering)
        {
            Chamber();
        }
    }

    public abstract void Ammunition();

    public void Chamber()
    {
        isChambering = true;
        reloadBar.Progress(1);
        Invoke("Chambering", chamberTime);
    }

    public void Chambering()
    {
        chamberedShots = chamberedShotsMax;
        isChambering = false;
        reloadBar.Progress(0);
    }
}