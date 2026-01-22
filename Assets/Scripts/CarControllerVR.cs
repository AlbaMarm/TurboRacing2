using System;
using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarControllerVR : MonoBehaviour
{

    [Header("Car Stats")]
    public float motorForce = 1500f;
    public float maxSpeed = 20f;
    public float turnSpeed = 1f;


    [Header("Inputs")]
    [SerializeField] InputActionReference rigthTrigger;
    [SerializeField] XRKnob steeringWheel;

    //Input variables
    bool accelerationTriggerPressed = false;
    Quaternion rotationValue;
    float currentSteeringAngle;

    float defaultRotationY;
    //References
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        steeringWheel = rb.GetComponentInChildren<XRKnob>();
        defaultRotationY = transform.rotation.y;
    }

    private void OnEnable()
    {
        rigthTrigger.action.performed += CarAccelate;
        rigthTrigger.action.canceled += CarAccelateStop;
    }


    private void OnDisable()
    {
        rigthTrigger.action.performed -= CarAccelate;
        rigthTrigger.action.canceled -= CarAccelateStop;
    }


    private void CarAccelate(InputAction.CallbackContext context)
    {
        Debug.Log("Gatillo presionado");
        accelerationTriggerPressed = true;
    }

    private void CarAccelateStop(InputAction.CallbackContext context)
    {
        Debug.Log("Gatillo soltado");
        accelerationTriggerPressed = false;
    }

    void FixedUpdate()
    {
        float targetSteeringAngle = (steeringWheel.maxAngle * (steeringWheel.value)) * turnSpeed;

        currentSteeringAngle = targetSteeringAngle;

        Quaternion targetRotation = Quaternion.Euler(0f, currentSteeringAngle + defaultRotationY, 0f);
        transform.rotation = targetRotation;

        // Control de aceleración con gatillo
        if (accelerationTriggerPressed)
        {
            rb.AddForce(transform.forward * motorForce * Time.fixedDeltaTime);
        }

        // Limitar la velocidad máxima (en magnitud)
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }
    }
}

/*
 public class CarController : MonoBehaviour
{

    [Header("Car Stats")]
    public float motorForce = 1500f;
    public float maxSpeed = 20f;
    public float turnSpeed = 100f; // Ajusta este valor según necesidad

    [Header("Inputs")]
    [SerializeField] InputActionReference rigthTrigger;
    //[SerializeField] InputActionReference rigthRotation;
    [SerializeField] XRKnob steeringWheel;

    //Input variables
    bool accelerationTriggerPressed = false;
    Quaternion rotationValue;

    float defaultRotationY;
    //References
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        steeringWheel = rb.GetComponentInChildren<XRKnob>();
        defaultRotationY = transform.rotation.y;
    }

    private void OnEnable()
    {
        rigthTrigger.action.performed += CarAccelate;
        rigthTrigger.action.canceled += CarAccelateStop;

        //rigthRotation.action.performed += CarRotate;
        //rigthRotation.action.canceled += CarRotate;
    }


    private void OnDisable()
    {
        rigthTrigger.action.performed -= CarAccelate;
        rigthTrigger.action.canceled -= CarAccelateStop;

        //rigthRotation.action.performed -= CarRotate;
        //rigthRotation.action.canceled -= CarRotate;
    }


    private void CarAccelate(InputAction.CallbackContext context)
    {
        Debug.Log("Gatillo presionado");
        accelerationTriggerPressed = true;
    }

    private void CarAccelateStop(InputAction.CallbackContext context)
    {
        Debug.Log("Gatillo soltado");
        accelerationTriggerPressed = false;
    }

    private void CarRotate(InputAction.CallbackContext context)
    {
        rotationValue =  context.ReadValue<Quaternion>();
        Debug.Log(rotationValue);
    }

    void FixedUpdate()
    {
        // Control de aceleración con gatillo
        if (accelerationTriggerPressed)
        {
            rb.AddForce(transform.forward * motorForce * Time.fixedDeltaTime);
        }

        // Control de velocidad máxima
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, Mathf.Min(maxSpeed, rb.linearVelocity.z));

        // Control de giro con knob
        float targetSteeringAngle = steeringWheel.maxAngle * steeringWheel.value;
        float turn = targetSteeringAngle * turnSpeed * Time.fixedDeltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }


}
 */

/*
 
 public class CarController : MonoBehaviour
{

    [Header("Car Stats")]
    public float motorForce = 1500f;
    public float maxSpeed = 20f;
    public float steeringSpeed = 90f; // Velocidad de giro en grados por segundo (ajustable)


    [Header("Inputs")]
    [SerializeField] InputActionReference rigthTrigger;
    //[SerializeField] InputActionReference rigthRotation;
    [SerializeField] XRKnob steeringWheel;

    //Input variables
    bool accelerationTriggerPressed = false;
    Quaternion rotationValue;

    float defaultRotationY;
    //References
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        steeringWheel = rb.GetComponentInChildren<XRKnob>();
        defaultRotationY = transform.rotation.y;
    }

    private void OnEnable()
    {
        rigthTrigger.action.performed += CarAccelate;
        rigthTrigger.action.canceled += CarAccelateStop;

        //rigthRotation.action.performed += CarRotate;
        //rigthRotation.action.canceled += CarRotate;
    }


    private void OnDisable()
    {
        rigthTrigger.action.performed -= CarAccelate;
        rigthTrigger.action.canceled -= CarAccelateStop;

        //rigthRotation.action.performed -= CarRotate;
        //rigthRotation.action.canceled -= CarRotate;
    }


    private void CarAccelate(InputAction.CallbackContext context)
    {
        Debug.Log("Gatillo presionado");
        accelerationTriggerPressed = true;
    }

    private void CarAccelateStop(InputAction.CallbackContext context)
    {
        Debug.Log("Gatillo soltado");
        accelerationTriggerPressed = false;
    }

    private void CarRotate(InputAction.CallbackContext context)
    {
        rotationValue =  context.ReadValue<Quaternion>();
        Debug.Log(rotationValue);
    }

    void FixedUpdate()
    {
        // Ángulo objetivo según el volante
        float targetSteeringAngle = steeringWheel.maxAngle * steeringWheel.value;

        // Girar suavemente el ángulo actual hacia el objetivo
        currentSteeringAngle = Mathf.MoveTowardsAngle(currentSteeringAngle, targetSteeringAngle, steeringSpeed * Time.fixedDeltaTime);

        // Aplicar la rotación al coche sumando la rotación inicial
        Quaternion targetRotation = Quaternion.Euler(0f, currentSteeringAngle + defaultRotationY, 0f);
        transform.rotation = targetRotation;

        // Control de aceleración con gatillo
        if (accelerationTriggerPressed)
        {
            rb.AddForce(transform.forward * motorForce * Time.fixedDeltaTime);
        }

        // Limitar la velocidad máxima (en magnitud)
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }
    }



}
 
 */


/*
        float knobValue = steeringWheelKnob != null ? steeringWheelKnob.value : 0f;
        float normalizedKnob = (knobValue * 2f) - 1f;
        float targetSteeringAngle = normalizedKnob * maxSteeringAngle;

        // Interpolación suave del ángulo de giro
        
        steeringAngle = Mathf.Lerp(steeringAngle, targetSteeringAngle, steeringSpeed * Time.fixedDeltaTime);

        // Obtener normal del terreno
        RaycastHit hit;
        Vector3 groundNormal = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            groundNormal = hit.normal;
        }

        // Dirección hacia adelante ajustada al terreno
        Vector3 forwardOnPlane = Vector3.ProjectOnPlane(transform.forward, groundNormal).normalized;

        // Calcular rotación objetivo con orientación al terreno
        Quaternion targetRotation = Quaternion.LookRotation(forwardOnPlane, groundNormal);

        // Aplicar giro del volante (yaw)
        // Extraer ángulo yaw actual
        float currentYaw = targetRotation.eulerAngles.y;
        // Añadir el ángulo de giro proporcional
        float speedFactor = Mathf.Clamp(rb.linearVelocity.magnitude / maxSpeed, 0f, 1f);
        float turnAmount = steeringAngle * speedFactor;
        float newYaw = currentYaw + turnAmount * Time.fixedDeltaTime;

        // Construir rotación final con nuevo yaw y la normal del terreno
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, newYaw, targetRotation.eulerAngles.z);

        // Aplicar rotación suavemente
        float rotationSpeed = 5f;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

 */

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
