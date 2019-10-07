using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    private TankController tankCtrl;
    private bool setupped = false;
    private float bulletSpeed;
    private int maxBounce;
    private int currentBounce;

    public void Setup(TankController _tankCtrl, float _bulletSpeed, int _maxBounce)
    {
        tankCtrl = _tankCtrl;
        bulletSpeed = _bulletSpeed;
        maxBounce = _maxBounce;
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
                }
            }

            Destroy(gameObject);
            return;
        }

        currentBounce++;
        if (currentBounce == maxBounce)
            Destroy(gameObject);
        else
        {
            Vector3 newDirection = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
        }
    }
}
