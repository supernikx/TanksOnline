using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITourret
{
    GameObject gameObject { get; }

    void Enable();
    void Setup(TankController _tankCtrl);
    void Shoot(Vector3 _aimDirection);
    void Disable();
}
