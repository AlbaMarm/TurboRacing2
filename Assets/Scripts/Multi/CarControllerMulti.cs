using Fusion;
using Fusion.Addons.Physics;
using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.XR;
using static Unity.Collections.Unicode;

public class CarControllerMulti : NetworkBehaviour
{
    [Header("Car Stats")]
    public float motorForce = 1500f;
    public float maxSpeed = 20f;
    public float turnSpeed = 1f;
    public float maxAngle = 3600.0f;

    private NetworkRigidbody3D rb;
    private float defaultRotationY;

    void Start()
    {
        rb = GetComponent<NetworkRigidbody3D>();
        defaultRotationY = transform.rotation.y;
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            GameObject controlVR = GameObject.FindGameObjectWithTag("ControlVR");

            if (controlVR!=null)
            {
                controlVR.transform.SetParent(this.transform);
                controlVR.transform.position = this.transform.position + new Vector3(0, 0.4F, -0.1f);
            }
        }

        /*
        if(HasStateAuthority)
        {
            transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);
        }
        */
    }

    void FixedUpdate()
    {
        if (HasStateAuthority)
        {
            if (GetInput(out InputData inputData))
            {
                float targetSteeringAngle = (maxAngle * (inputData.knobValue)) * turnSpeed;

                Quaternion targetRotation = Quaternion.Euler(0f, targetSteeringAngle + defaultRotationY, 0f);
                rb.Rigidbody.MoveRotation(targetRotation);

                // Control de aceleración con gatillo
                if (inputData.triggerPressed)
                {
                    rb.Rigidbody.AddForce((transform.forward * motorForce * Runner.DeltaTime) * 1);
                }

                // Limitar la velocidad máxima (en magnitud)
                Vector3 horizontalVelocity = new Vector3(rb.Rigidbody.linearVelocity.x, 0, rb.Rigidbody.linearVelocity.z);
                if (horizontalVelocity.magnitude > maxSpeed)
                {
                    horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
                    rb.Rigidbody.linearVelocity = new Vector3(horizontalVelocity.x, rb.Rigidbody.linearVelocity.y, horizontalVelocity.z);
                }

            }
        }

    }
}
