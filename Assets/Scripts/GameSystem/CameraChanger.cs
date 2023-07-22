using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BBGameStudios.Managers
{
    public class CameraChanger : MonoBehaviour
    {
        [SerializeField] private List<CinemachineVirtualCamera> virtualCameras;


        public void ChangeCameraTo(CinemachineVirtualCamera cam)
        {
            foreach (var c in virtualCameras)
            {
                c.Priority = 0;
            }

            cam.Priority = 10;
        }
    }
}

