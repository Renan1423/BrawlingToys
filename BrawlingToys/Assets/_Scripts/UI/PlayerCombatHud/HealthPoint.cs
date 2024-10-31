using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.UI
{
    public class HealthPoint : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        public void DisableHealthPoint() 
        {
            anim.SetTrigger("Disable");
        }
    }
}
