using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public static RespawnController i;

    [SerializeField]
    private TankController player1;
    [SerializeField]
    private TankController player2;
    [SerializeField]
    private List<Transform> spawnPositions;

    private void Awake()
    {
        i = this;

        player1.OnPlayerWin += HandlePlayerWin;
        player2.OnPlayerWin += HandlePlayerWin;
    }

    private void HandlePlayerWin(Player _player)
    {
        player1.OnPlayerWin -= HandlePlayerWin;
        player2.OnPlayerWin -= HandlePlayerWin;

        switch (_player)
        {
            case Player.P1:
                Destroy(player2.gameObject);
                break;
            case Player.P2:
                Destroy(player1.gameObject);
                break;
        }
    }

    public Transform GetRandomSpawnPoint()
    {
        return spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
    }
}
