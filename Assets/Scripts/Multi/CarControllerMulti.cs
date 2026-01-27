using Fusion;
using Fusion.Addons.Physics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.XR;
using static Unity.Collections.Unicode;
using UnityEngine.SceneManagement;
using System;

public class CarControllerMulti : NetworkBehaviour
{
    [Header("Car Stats")]
    public float motorForce = 1500f;
    public float maxSpeed = 20f;
    public float turnSpeed = 1f;
    public float maxAngle = 3600.0f;

    private NetworkRigidbody3D rb;
    private float defaultRotationY;
    private GameObject controlVR;

    void Start()
    {
        rb = GetComponent<NetworkRigidbody3D>();
        defaultRotationY = transform.rotation.y;
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            controlVR = GameObject.FindGameObjectWithTag("ControlVR");

            if (controlVR!=null)
            {
                controlVR.transform.SetParent(this.transform);
                controlVR.transform.position = this.transform.position;
            }
        }

        /*
        if(HasStateAuthority)
        {
            transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);
        }
        */
    }

    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority && controlVR != null)
        {
            controlVR.transform.position = this.transform.position;
        }

        if (HasStateAuthority)
        {
            if (GetInput(out InputData inputData))
            {
                float targetSteeringAngle = (maxAngle * (inputData.knobValue)) * turnSpeed;

                //Debug.Log(inputData.knobValue);
                //Debug.Log(inputData.triggerPressed);

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

            
            ReadOnlyDictionary<string, SessionProperty> ganador = Runner.SessionInfo.Properties;

            if (ganador.TryGetValue("Ganador", out SessionProperty data))
            {
                int numGanador = (int)data.PropertyValue;
                if (numGanador != 0)//&& Runner.LocalPlayer.IsRealPlayer
                {

                    int nextScene;
                    if(Runner.IsSceneAuthority && numGanador == Runner.LocalPlayer.AsIndex)
                    {
                        Debug.Log("gana");
                        nextScene = 4;
                        //Runner.UnloadScene(Runner.SceneManager.GetSceneRef(SceneManager.GetSceneByBuildIndex(3).name));
                        //Runner.LoadScene(Runner.SceneManager.GetSceneRef(SceneManager.GetSceneByBuildIndex(4).name));
                    }
                    else
                    {
                        Debug.Log("pierde");
                        nextScene = 5;
                    }
                    SceneManager.LoadScene(nextScene);
                    SceneManager.sceneLoaded += DesconectarRed;
                }
            }
            

        }

    }

    private void DesconectarRed(Scene arg0, LoadSceneMode arg1)
    {
        Runner.Shutdown(); // async pero aquí vale llamarlo así
        SceneManager.sceneLoaded -= DesconectarRed;

    }
}
