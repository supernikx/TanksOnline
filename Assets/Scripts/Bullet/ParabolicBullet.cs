using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicBullet : MonoBehaviour
{
    [Header("ParabolicBullet Settings")]
    [SerializeField]
    float gravity;

    private float travelTime;
    private float yVelocity;
    private float xVelocity;
    private float zVelocity;
    private TankController tankCtrl;
    private bool setupped = false;

    public void Setup(TankController _tankCtrl, float _bulletSpeed, float _bulletAngle, Vector3 _aimDirection)
    {
        tankCtrl = _tankCtrl;
        xVelocity = (_bulletSpeed * Mathf.Cos(_bulletAngle * Mathf.Deg2Rad)) * _aimDirection.x;
        zVelocity = (_bulletSpeed * Mathf.Cos(_bulletAngle * Mathf.Deg2Rad)) * _aimDirection.z;
        yVelocity = _bulletSpeed * Mathf.Sin(_bulletAngle * Mathf.Deg2Rad);
        travelTime = 0;
        ColorChangeDebug();
        setupped = true;
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

    private void FixedUpdate()
    {
        if (!setupped)
            return;

        Vector3 _movementDirection = new Vector3(xVelocity, (yVelocity - (gravity * travelTime)), zVelocity);
        //Calcolo la rotazione in base al movimento del proiettile e la applico
        float zRotation = Mathf.Atan2(_movementDirection.y, _movementDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, zRotation);

        transform.position += _movementDirection * Time.deltaTime;
        travelTime += Time.deltaTime;
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
}
