using UnityEngine;

public class CutTree : MonoBehaviour
{
    public GameObject onCollectEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(onCollectEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
           
    }
}
