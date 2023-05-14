using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PickupController))]
public class KnifeHit : MonoBehaviour
{
    private PickupController pickupController;

    private void Start()
    {
        pickupController = GetComponent<PickupController>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if(pickupController.TryToPickup())
        {
            return;
        }
        var tmp = other.gameObject.GetComponent<YoungerEnemyAI>();
        if(tmp != null)
        {
            tmp.GetKilledServerRPC();
            Destroy(gameObject);
        }
    }
}
