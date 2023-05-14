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
        pickupController.OnEnemyCollisionEvent += (_, collision) => OnEnemyCollision(collision);
    }

    private void OnEnemyCollision(Collision other)
    {
        var tmp = other.gameObject.GetComponent<YoungerEnemyAI>();
        if(tmp != null)
        {
            tmp.GetKilledServerRPC();
            Destroy(gameObject);
        }
    }
}
