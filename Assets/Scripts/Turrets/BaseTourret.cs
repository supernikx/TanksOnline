using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTourret : MonoBehaviour, ITourret
{
    [SerializeField]
    private GameObject torretGraphic;
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private float shootSpeed;
    [SerializeField]
    private float firingRate;

    private TankController tankCtrl;
    private IEnumerator firingRateRoutine;
    private bool canShot;
    private bool enable;

    public void Setup(TankController _tankCtrl)
    {
        tankCtrl = _tankCtrl;
    }

    public void Enable()
    {
        canShot = true;
        enable = true;
        torretGraphic.SetActive(true);
    }


    public void Shoot(Vector3 _aimDirection)
    {
        if (!enable || !canShot)
            return;

        Bullet bullet = Instantiate(bulletPrefab, shootPosition.position, Quaternion.LookRotation(_aimDirection, Vector3.up));
        bullet.Setup(tankCtrl, shootSpeed);

        firingRateRoutine = FiringRateCoroutine();
        StartCoroutine(firingRateRoutine);
    }

    private IEnumerator FiringRateCoroutine()
    {
        canShot = false;
        yield return new WaitForSeconds(1f / firingRate);
        canShot = true;
    }

    public void Disable()
    {
        if (firingRateRoutine != null)
            StopCoroutine(firingRateRoutine);

        canShot = false;
        enable = false;
        torretGraphic.SetActive(false);
    }
}
