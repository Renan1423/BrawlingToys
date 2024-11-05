using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.Core
{
    public class LookAtCamera : MonoBehaviour
    {
        private enum LookAtMode
        {
            LookAt,
            LookAtInverted,
            CameraFoward,
            CameraFowardInverted
        }

        [SerializeField] private LookAtMode lookAtMode;

        private void LateUpdate()
        {
            switch (lookAtMode)
            {
                case LookAtMode.LookAt:
                    transform.LookAt(Camera.main.transform);
                    break;
                case LookAtMode.LookAtInverted:
                    Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;
                case LookAtMode.CameraFoward:
                    transform.forward = Camera.main.transform.forward;
                    break;
                case LookAtMode.CameraFowardInverted:
                    transform.forward = -Camera.main.transform.forward;
                    break;
            }
        }
    }
}
