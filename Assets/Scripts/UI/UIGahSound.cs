using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIGahSound : Singleton<UIGahSound>
    {
        [Header("References")]
        [SerializeField]
        private UIBarController barController;

        public void UpdateBar(float a)
        {
            barController.ChangeValue(a, 1);
        }
    }
}