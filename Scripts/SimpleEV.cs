using UnityEngine;
using UnityEngine.AI;

public class SimpleEV : MonoBehaviour
{
    [Header("1. Assign The Car Parts")]
    public NavMeshAgent carAgent;
    public BoxCollider carDoorCollider; // The invisible box around the car body

    [Header("2. Assign The Destination Button")]
    public GameObject buttonCVRaman;    // The 3D Cube/Button for CV Raman
    public Transform pointCVRaman;      // The empty GameObject where Car should stop

    [Header("3. Assign VR Player Parts")]
    public Transform vrCameraRig;       // Your [BuildingBlock] Camera Rig
    public Transform rightHandAnchor;   // The Right Controller Anchor (Laser origin)
    
    [Header("4. Positions")]
    public Transform seatPoint;         // Where you sit
    public Transform exitPoint;         // Where you get out

    private bool isSitting = false;

    void Update()
    {
        // LISTEN FOR CLICK (Right Trigger)
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            CheckWhatWeHit();
        }
        
        // LISTEN FOR EXIT (X Button)
        if (isSitting && (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three)))
        {
            ExitCar();
        }

        // KEEP PLAYER IN SEAT
        if (isSitting)
        {
            vrCameraRig.position = seatPoint.position;
            vrCameraRig.rotation = seatPoint.rotation;
        }
    }

    void CheckWhatWeHit()
    {
        // Shoot a ray from the hand
        Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            // DID WE HIT THE BUTTON? (Priority 1)
            if (hit.collider.gameObject == buttonCVRaman)
            {
                Debug.Log("Button Clicked: Going to CV Raman");
                carAgent.SetDestination(pointCVRaman.position);
            }
            // DID WE HIT THE CAR DOOR? (Priority 2)
            else if (hit.collider == carDoorCollider && !isSitting)
            {
                Debug.Log("Car Clicked: Getting In");
                EnterCar();
            }
        }
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
    }
}