using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.XR;

public class CarController : MonoBehaviour
{
    public XRKnob steeringWheelKnob;

    public float maxSteeringAngle = 30f; 
    public float motorForce = 1500f;     
    public float maxSpeed = 20f;          

    public float steeringAngle;

    private Rigidbody rb;
    private InputDevice rightController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Obtener controlador derecho
        var rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
        {
            rightController = rightHandedControllers[0];
        }
    }

    void FixedUpdate()
    {
        float knobValue = steeringWheelKnob != null ? steeringWheelKnob.value : 0f;

        steeringAngle = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, knobValue);

        Quaternion turnRotation = Quaternion.Euler(rb.rotation.x, steeringAngle * 100 * Time.fixedDeltaTime, rb.rotation.z);
        rb.MoveRotation(turnRotation);


        //rb.AddForce((transform.forward * motorForce * Time.fixedDeltaTime) * 1);

        // Detectar gatillo presionado para avanzar

        if (rightController.isValid)
        {
            bool triggerPressed = false;
            if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
            {
                if (rb.linearVelocity.magnitude < maxSpeed)
                {
                    rb.AddForce((transform.forward * motorForce * Time.fixedDeltaTime) * 1);
                }
            }
        }
    }
}


/*
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class VRCarPhysicsController : MonoBehaviour
{
    public XRKnob steeringWheelKnob;

    public float maxSteeringAngle = 30f; // M�ximo �ngulo de giro en grados
    public float motorForce = 1500f;     // Fuerza aplicada para avanzar
    public float maxSpeed = 20f;          // Velocidad m�xima en m/s

    private Rigidbody rb;
    private InputDevice rightController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Obtener controlador derecho
        var rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
        {
            rightController = rightHandedControllers[0];
        }
    }

    void FixedUpdate()
    {
        // Obtener valor del knob (0 a 1)
        float knobValue = steeringWheelKnob != null ? steeringWheelKnob.value : 0f;

        // Mapear a �ngulo de giro entre -maxSteeringAngle y +maxSteeringAngle
        float steeringAngle = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, knobValue);

        // Aplicar rotaci�n suave al Rigidbody (girando alrededor del eje Y)
        Quaternion turnRotation = Quaternion.Euler(0f, steeringAngle * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // Detectar gatillo presionado para avanzar
        if (rightController.isValid)
        {
            bool triggerPressed = false;
            if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
            {
                // Solo aplicar fuerza si no se supera la velocidad m�xima
                if (rb.velocity.magnitude < maxSpeed)
                {
                    // Aplicar fuerza hacia adelante en la direcci�n actual del coche
                    rb.AddForce(transform.forward * motorForce * Time.fixedDeltaTime);
                }
            }
        }
    }
}

*/
