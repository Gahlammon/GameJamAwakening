using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAimer : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        private float aimSensitivity = 1;

        public Vector3 AimDirection => new Vector3(aim.x, 0, aim.y).normalized;

        private Vector2 aim = Vector2.up;
        private Vector2 aimSpeed = Vector2.zero;

        public void SetAimSpeed(Vector2 aimSpeed)
        {
            this.aimSpeed = aimSpeed;
        }

        private void Update()
        {
            aim += aimSpeed * aimSensitivity * Time.deltaTime;
            if(aim.magnitude > 1)
            {
                aim.Normalize();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + (AimDirection * 5));
        }
    }
}