using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupController : MonoBehaviour
{
    private bool isPickedUp = false;
    private bool isThrown = false;

    private new Rigidbody rigidbody;

    private void Start()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    public bool TryToPickup()
    {
        if (isPickedUp || isThrown)
        {
            return false;
        }
        isPickedUp = true;
        return true;
    }

    public void Throw(Vector3 force)
    {
        isThrown = true;
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown)
        {
            isThrown = false;
            print("Jebs!");
        }
    }
}
