using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupController : NetworkBehaviour
{
    public enum PickupType
    {
        Bootle,
        Knife,
        Brick,
        Pipe,
        Part,
        Medalion
    }

    [SerializeField]
    private PickupType type;
    [SerializeField]
    private Sprite uISprite;

    public PickupType Type => type;
    public Sprite UISprite => uISprite;

    private bool isThrown = false;

    private new Rigidbody rigidbody;
    public event System.EventHandler OnCollisionEvent;
    public event System.EventHandler<Collision> OnEnemyCollisionEvent;

    private void Start()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    public bool TryToPickup()
    {
        return !isThrown;
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnEnemyCollisionEvent?.Invoke(this, collision);
            return;
        }
        if (isThrown)
        {
            OnCollisionEvent?.Invoke(this, null);
            isThrown = false;
            print("Jebs!");
        }
    }
}
