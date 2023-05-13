using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollower : Singleton<CameraPlayerFollower>
{
    [Header("Config")]
    [SerializeField]
    private float distance;

    private Transform playerTransform = null;

    public void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
    }

    private void LateUpdate()
    {
        if (playerTransform == null)
        {
            return;
        }

        transform.position = playerTransform.position - distance * transform.forward;
    }
}
