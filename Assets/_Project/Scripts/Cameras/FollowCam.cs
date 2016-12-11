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
        private Rigidbody _rigidBody;

        void Start()
        {
            _cam = GetComponent<Camera>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (_cam == null) return;


            var desiredPosition = Target.position + (-Target.forward * Distance) + (Vector3.up * Height);
            var currentPosition = transform.position;
            var targetPosition = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * Damping);
            if (_rigidBody != null)
            {
                _rigidBody.MovePosition(targetPosition);
                _rigidBody.MoveRotation(Quaternion.LookRotation(Target.position - transform.position));
            }
            else
            {
                transform.position = targetPosition;
                transform.LookAt(Target.position);
            }
        }
    }
}
