using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIThrowController : Singleton<UIThrowController>
    {
        [Header("References")]
        [SerializeField]
        private UIImageFadeController fadeController;
        [SerializeField]
        private UIBarController barController;

        [Header("Config")]
        [SerializeField]
        private float fadeDuration;

        private float maxThrowHoldTime;
        private float counter = 0;
        private bool isHolding = false;

        private void Start()
        {
            fadeController.Fade(false, fadeDuration);
        }

        private void Update()
        {
            if (!isHolding)
            {
                return;
            }
            counter += Time.deltaTime;
            barController.ChangeValue(counter, maxThrowHoldTime);
        }

        private void OnThrowStart()
        {
            counter = 0;
            isHolding = true;
            fadeController.Fade(true, fadeDuration);
            barController.ChangeValue(0, maxThrowHoldTime);
        }

        private void OnThrowEnd()
        {
            isHolding = false;
            fadeController.Fade(false, fadeDuration);
        }

        public void SetPlayerInputAdapter(Player.PlayerInputAdapter playerInputAdapter)
        {
            maxThrowHoldTime = playerInputAdapter.MaxHoldTime;
            playerInputAdapter.ThrowStartEvent += (_, _) => OnThrowStart();
            playerInputAdapter.ThrowEndEvent += (_, _) => OnThrowEnd();
        }
    }
}