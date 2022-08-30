using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    public void Shoot();
    public void ChamberShot();
    public void BrokenShoot();
}