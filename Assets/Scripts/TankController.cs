using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{
    public Action<Player> OnPlayerWin;

    [SerializeField]
    Player player;
    [SerializeField]
    private GameObject graphic;
    [SerializeField]
    private GameObject winText;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float respawnTime;

    List<ITourret> tourrets = new List<ITourret>();
    int tourretIndex;

    Rigidbody rb;
    new BoxCollider collider;
    bool readInput;
    Vector3 movementDir;
    Vector3 aimDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        tourrets = GetComponentsInChildren<ITourret>(true).ToList();
        tourretIndex = 0;

        foreach (var t in tourrets)
        {
            t.Setup(this);
            t.Disable();
        }

        tourrets[0].Enable();
        readInput = true;
    }

    private void Reset()
    {
        tourrets[tourretIndex].Disable();
        tourretIndex = 0;
        tourrets[tourretIndex].Enable();
    }

    private void Update()
    {
        if (!readInput)
            return;

        movementDir = new Vector3(Input.GetAxis(player + "HorizontalMove"), 0, Input.GetAxis(player + "VerticalMove"));
        aimDir = new Vector3(Input.GetAxis(player + "HorizontalAim"), 0, Input.GetAxis(player + "VerticalAim"));

        switch (player)
        {
            case Player.P1:
                if (Input.GetKey(KeyCode.Joystick1Button5))
                {
                    Shoot();
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                {
                    ChangeWeapon();
                }
                break;
            case Player.P2:
                if (Input.GetKey(KeyCode.Joystick2Button5))
                {
                    Shoot();
                }

                if (Input.GetKeyDown(KeyCode.Joystick2Button4))
                {
                    ChangeWeapon();
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!readInput)
            return;

        Move();
        Aim();
    }

    private void Move()
    {
        if (movementDir != Vector3.zero)
        {
            transform.Translate(movementDir * movementSpeed);
            graphic.transform.rotation = Quaternion.LookRotation(movementDir, Vector3.up);
        }
    }

    private void Aim()
    {
        if (aimDir != Vector3.zero)
            tourrets[tourretIndex].gameObject.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
    }

    private void Shoot()
    {
        tourrets[tourretIndex].Shoot(aimDir);
    }

    private IEnumerator RespawnCoroutine()
    {
        rb.useGravity = false;
        collider.enabled = false;
        tourrets[tourretIndex].Disable();
        graphic.SetActive(false);
        readInput = false;
        yield return new WaitForSeconds(respawnTime);
        graphic.SetActive(true);
        readInput = true;
        collider.enabled = true;
        rb.useGravity = true;
        tourrets[tourretIndex].Enable();
    }

    #region API
    public void ChangeWeapon()
    {
        tourrets[tourretIndex].Disable();
        tourretIndex++;
        if (tourretIndex > tourrets.Count - 1)
        {
            tourretIndex--;
            winText.SetActive(true);
            OnPlayerWin?.Invoke(player);
        }
        tourrets[tourretIndex].Enable();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void Kill()
    {
        Reset();
        transform.position = RespawnController.i.GetRandomSpawnPoint().position;
        StartCoroutine(RespawnCoroutine());
    }
    #endregion
}

public enum Player
{
    P1,
    P2,
}
