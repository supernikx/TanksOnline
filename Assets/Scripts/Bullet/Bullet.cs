using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private TankController tankCtrl;
    private bool setupped = false;
    private float bulletSpeed;

    public void Setup(TankController _tankCtrl, float _bulletSpeed)
    {
        tankCtrl = _tankCtrl;
        bulletSpeed = _bulletSpeed;
        ColorChangeDebug();
        setupped = true;
    }

    private void ColorChangeDebug()
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        switch (tankCtrl.GetPlayer())
        {
            case Player.P1:
                meshRend.material.color = Color.cyan;
                break;
            case Player.P2:
                meshRend.material.color = Color.red;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!setupped)
            return;

        transform.Translate(Vector3.forward * bulletSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tank"))
        {
            TankController hitTank = collision.gameObject.GetComponent<TankController>();
            if (hitTank)
            {
                if (hitTank.GetPlayer() != tankCtrl.GetPlayer())
                {
                    tankCtrl.ChangeWeapon();
                    hitTank.Kill();
                    Destroy(gameObject);
                }
            }
            return;
        }

        Destroy(gameObject);
    }
}
