using Fusion;
using Fusion.Addons.Physics;
using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.XR;
using static Unity.Collections.Unicode;

public class CarControllerMulti : NetworkBehaviour
{
   
    
    public float maxSteeringAngle = 30f; 
    public float motorForce = 1500f;     
    public float maxSpeed = 20f;          

    public float steeringAngle;

    private NetworkRigidbody3D rb;

    void Start()
    {
        rb = GetComponent<NetworkRigidbody3D>();

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

        if(HasStateAuthority)
        {
            transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);
        }
    }

    void FixedUpdate()
    {
        if (HasStateAuthority)
        {
            if (GetInput(out InputData inputData))
            {

                steeringAngle = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, inputData.knobValue);

                Quaternion turnRotation = Quaternion.Euler(rb.Rigidbody.rotation.x, steeringAngle * 100 * Time.fixedDeltaTime, rb.Rigidbody.rotation.z);
                rb.Rigidbody.MoveRotation(turnRotation);

                

                if (inputData.triggerPressed)
                {
                    if (rb.Rigidbody.linearVelocity.magnitude < maxSpeed)
                    {
                        rb.Rigidbody.AddForce((transform.forward * motorForce * Time.fixedDeltaTime) * 1);
                    }
                }
            }
        }

    }
}
