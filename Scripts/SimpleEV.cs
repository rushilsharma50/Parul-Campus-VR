using UnityEngine;
using UnityEngine.AI;

public class SimpleEV : MonoBehaviour
{
    [Header("1. Assign The Car Parts")]
    public NavMeshAgent carAgent;
    public BoxCollider carDoorCollider; 

    [Header("2. Assign The Destination")]
    public GameObject buttonCVRaman;    
    public Transform pointCVRaman;      

    [Header("3. Assign VR Player Parts")]
    public Transform vrCameraRig;       
    public Transform rightHandAnchor;   
    
    [Header("4. Positions")]
    public Transform seatPoint;         
    public Transform exitPoint;         

    [Header("5. Interaction Settings")]
    public LayerMask buttonLayer; // Assign "CarButtons" here
    public float rayDistance = 50f; // Longer range

    private bool isSitting = false;

    void Update()
    {
        // DEBUG VISUALIZER: Draws a red line in Scene View showing where you point
        Debug.DrawRay(rightHandAnchor.position, rightHandAnchor.forward * 10f, Color.red);

        // LISTEN FOR CLICK (Right Trigger)
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            CheckWhatWeHit();
        }
        
        // LISTEN FOR EXIT (X or A Button)
        if (isSitting && (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three)))
        {
            ExitCar();
        }

        // LOCK PLAYER TO SEAT
        if (isSitting)
        {
            vrCameraRig.position = seatPoint.position;
            vrCameraRig.rotation = seatPoint.rotation;
        }
    }

    void CheckWhatWeHit()
    {
        Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);
        RaycastHit hit;

        // DEBUG: Draw the ray so you can see it in Scene View
        Debug.DrawRay(rightHandAnchor.position, rightHandAnchor.forward * 50f, Color.red, 1.0f);

        if (isSitting)
        {
            // Only checking the "CarButtons" layer
            if (Physics.Raycast(ray, out hit, rayDistance, buttonLayer))
            {
                Debug.Log("Hit on Button Layer: " + hit.collider.name); // <--- THIS TELLS THE TRUTH

                if (hit.collider.gameObject == buttonCVRaman)
                {
                    Debug.Log("SUCCESS: Library Button Clicked!");
                    DriveTo(pointCVRaman.position);
                }
            }
            else
            {
                Debug.Log("Missed! Ray went through everything and hit nothing on 'CarButtons' layer.");
            }
        }
        else 
        {
            // Outside logic...
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider == carDoorCollider) EnterCar();
            }
        }
    }

    void DriveTo(Vector3 targetPosition)
    {
        // Force Wake Up the Agent
        carAgent.isStopped = false; 
        carAgent.SetDestination(targetPosition);
    }

    void EnterCar()
    {
        isSitting = true;
        vrCameraRig.SetParent(seatPoint);
    }

    void ExitCar()
    {
        isSitting = false;
        vrCameraRig.SetParent(null);
        vrCameraRig.position = exitPoint.position;
        vrCameraRig.rotation = Quaternion.identity;
        
        // Stop the car when we get out (Optional)
        carAgent.isStopped = true;
    }
}