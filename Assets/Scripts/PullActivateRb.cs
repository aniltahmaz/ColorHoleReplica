using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullActivateRb : MonoBehaviour
{
    public static PullActivateRb Instance;

    [SerializeField] float pullForce;
    List<Rigidbody> affectedRigidbodies = new List<Rigidbody>();
    Transform holeCenter;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    void Start()
    {
        holeCenter = transform;
        affectedRigidbodies.Clear();
        
    }

    //Adding the triggered object to the list

    private void OnTriggerEnter(Collider other)
    {
        if(!GameController.GameOver&&!GameController.moveHoleToNext&&!GameController.NextStage&&other.transform.root.tag.Equals("Collectable1")
            || other.transform.root.tag.Equals("Collectable2") || other.transform.root.tag.Equals("NonCollectable"))
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            ActivateAndPull(other.attachedRigidbody);
            
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!GameController.GameOver && !GameController.moveHoleToNext && !GameController.NextStage && other.transform.root.tag.Equals("Collectable1")
            || other.transform.root.tag.Equals("Collectable2") || other.transform.root.tag.Equals("NonCollectable"))
        {
            
            RemoveFromPull(other.attachedRigidbody);


        }
    }

    //Apply force to objects in the list towards to the hole 
    private void FixedUpdate()
    {
        if (!GameController.GameOver && !GameController.NextStage && !GameController.moveHoleToNext)
        {
            foreach(Rigidbody rb in affectedRigidbodies)
            {
                rb.AddForce((holeCenter.position - rb.position) * pullForce * Time.fixedDeltaTime);
            }
        }
    }
    public void ActivateAndPull(Rigidbody rb)
    {
        affectedRigidbodies.Add(rb);
    }
    public void RemoveFromPull(Rigidbody rb)
    {
        affectedRigidbodies.Remove(rb);

    }
}
