using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UICrosshair : Singleton<UICrosshair>
    {
        [Header("Config")]
        [SerializeField]
        private float radious;

        private Player.PlayerAimer playerAimer;

        public void SetPlayerAimer(Player.PlayerAimer playerAimer)
        {
            this.playerAimer = playerAimer;
        }

        private void Update()
        {
            if (playerAimer == null)
            {
                return;
            }

            Vector3 newPos = playerAimer.AimDirection * radious;
            transform.localPosition = new Vector3(newPos.x, newPos.z, 0);
        }
    }
}