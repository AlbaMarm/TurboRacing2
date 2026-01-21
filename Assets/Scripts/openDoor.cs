using UnityEngine;

public class openDoor : MonoBehaviour
{
    public GameObject puerta;
    public HingeJoint joint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joint = puerta.GetComponent<HingeJoint>();
        joint.useMotor = false;
    }

    public void open()
    {
        joint.useMotor = true;
        Debug.Log("Abriendo puertita");
    }

    public void close()
    {
        joint.useMotor = false;
        Debug.Log("Cerrando puertita");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
