using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PickupController))]
public class BootleHit : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius = 5;
    private PickupController pickupController;
    private Explode explode;

    private void Start()
    {
        pickupController = GetComponent<PickupController>();
        pickupController.OnCollisionEvent += (_, _) => OnHit();
        pickupController.OnEnemyCollisionEvent += (_, collision) => OnEnemyHit(collision);
        explode = GetComponentInChildren<Explode>();
    }
    private void OnHit()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider hit in hits)
        {
            print(hit.gameObject.name);
            var tmp = hit.gameObject.GetComponent<YoungerEnemyAI>();
            if(tmp != null)
            {
                tmp.GetKilledServerRPC();
            }
        }
        explode.StartExplode();
    }

    private void OnEnemyHit(Collision collision)
    {
        var tmp = collision.gameObject.GetComponent<YoungerEnemyAI>();
        if(tmp != null)
        {
            tmp.GetKilledServerRPC();
        }
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider hit in hits)
        {
            tmp = hit.gameObject.GetComponent<YoungerEnemyAI>();
            if(tmp != null)
            {
                tmp.GetKilledServerRPC();
            }
        }
        explode.StartExplode();
    }
}
