using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        PlayerController.LocalPlayerJoined += PlayerJoined;
    }

    private void OnDisable()
    {
        PlayerController.LocalPlayerJoined -= PlayerJoined;
    }

    private void PlayerJoined(Transform player)
    {
        _vcam.Follow = player;
    }
}
