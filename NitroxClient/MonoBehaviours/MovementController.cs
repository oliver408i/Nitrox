using NitroxClient.GameLogic;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NitroxClient.MonoBehaviours
{
    public class MovementController : MonoBehaviour
    {
        public float Scalar { get; set; } = 1f;
        public Vector3 TargetPosition { get; set; }
        public Quaternion TargetRotation { get; set; }
        public Transform TargetTransform { get; set; }

        public event Action BeforeUpdate = () => {};
        public event Action BeforeFixedUpdate = () => {};
        public event Action AfterUpdate = () => {};
        public event Action AfterFixedUpdate = () => {};

        private Rigidbody rigidbody;
        public Vector3 Velocity;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            BeforeUpdate();
            if (!rigidbody)
            {
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, Scalar * Time.fixedDeltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Scalar * Time.deltaTime);
            }
            AfterUpdate();
        }

        private void FixedUpdate()
        {
            BeforeFixedUpdate();
            if (rigidbody)
            {
                float timing = Scalar * Time.fixedDeltaTime;
                Vector3 newPos = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, timing);
                Quaternion newRot = Quaternion.Lerp(transform.rotation, TargetRotation, timing);

                if (rigidbody.isKinematic)
                {
                    rigidbody.MovePosition(newPos);
                    rigidbody.MoveRotation(newRot);
                }
                else
                {
                    rigidbody.velocity = Velocity;

                    Quaternion delta = TargetRotation * transform.rotation.GetInverse();
                    delta.ToAngleAxis(out float angle, out Vector3 axis);
                    rigidbody.angularVelocity = Mathf.Deg2Rad * angle / timing * axis;
                }
            }
            AfterFixedUpdate();
        }
    }
}
