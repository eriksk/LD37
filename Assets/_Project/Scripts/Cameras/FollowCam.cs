using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class FollowCam : MonoBehaviour
    {
        public Transform Target;
        public float Damping = 5f;
        public float Height = 2f;
        public float Distance = 10f;

        private Camera _cam;

        void Start()
        {
            _cam = GetComponent<Camera>();
        }

        void Update()
        {
            if (_cam == null) return;


            var desiredPosition = Target.position + (-Target.forward * Distance) + (Vector3.up * Height);
            var currentPosition = transform.position;
            transform.position = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * Damping);
            transform.LookAt(Target.position);
        }
    }
}
