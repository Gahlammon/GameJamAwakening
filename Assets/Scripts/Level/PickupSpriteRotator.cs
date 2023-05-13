using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpriteRotator : MonoBehaviour
{
    private void Start()
    {
        transform.forward = -CameraPlayerFollower.Instance.transform.forward;
    }
}
