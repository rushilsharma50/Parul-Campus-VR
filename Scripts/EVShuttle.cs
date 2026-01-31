using UnityEngine;
using UnityEngine.AI;

public class EVShuttle : MonoBehaviour
{
    [Header("Setup")]
    public NavMeshAgent carAgent;
    public GameObject destinationUI; 
    public BoxCollider carCollider;  

    [Header("VR Player Setup")]
    public Transform vrCameraRig;     
    public Transform rightHandAnchor; 
    
    [Header("Positions")]
    public Transform seatPoint;       
    public Transform exitPoint;       

    [Header("Destinations")]
    public Transform[] buildingEntrances; 

    [Header("UI Buttons")]
    public GameObject btnLibrary;
    public GameObject btnHostel;

    private bool isSitting = false;
    private bool isMoving = false;

    void Update()
    {
        // Check for Click (Right Index Trigger)
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            // Debug 1: Did we even press the trigger?
            Debug.Log("Trigger Pressed! Firing Ray...");

            Ray ray = new Ray(rightHandAnchor.position, rightHandAnchor.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20f)) // Increased range to 20m
            {
                // Debug 2: What did we hit?
                Debug.Log("Ray hit object: " + hit.collider.gameObject.name);

                // LOGIC: CLICK BUTTONS (Only if inside)
                if (isSitting)
                {
                    if (hit.collider.gameObject == btnLibrary)
                    {
                        Debug.Log(">>> LIBRARY BUTTON CLICKED! <<<");
                        GoToBuilding(0); 
                    }
                    else if (hit.collider.gameObject == btnHostel)
                    {
                        Debug.Log(">>> HOSTEL BUTTON CLICKED! <<<");
                        GoToBuilding(1); 
                    }
                    else 
                    {
                        Debug.Log("Hit something, but it was not a button.");
                    }
                }
                // LOGIC: ENTER CAR
                else if (hit.collider == carCollider)
                {
                    Debug.Log(">>> CAR CLICKED! Entering... <<<");
                    EnterVehicle();
                }
            }
            else
            {
                Debug.Log("Ray hit NOTHING. Aim closer or check colliders.");
            }
        }
    }

    public void GoToBuilding(int buildingIndex)
    {
        // Debug 3: Logic Checks
        if (buildingEntrances.Length == 0)
        {
            Debug.LogError("ERROR: No Building Entrances assigned in Inspector!");
            return;
        }

        if (carAgent == null || !carAgent.isOnNavMesh)
        {
            Debug.LogError("ERROR: Car is not on the NavMesh! Bake your Navigation or move the car.");
            return;
        }

        Debug.Log("Driving to: " + buildingEntrances[buildingIndex].name);
        destinationUI.SetActive(false); 
        carAgent.SetDestination(buildingEntrances[buildingIndex].position);
        isMoving = true; 
    }

    // ... (Keep EnterVehicle and ExitVehicle the same as before) ...
    public void EnterVehicle()
    {
        isSitting = true;
        vrCameraRig.position = seatPoint.position;
        vrCameraRig.rotation = seatPoint.rotation;
        vrCameraRig.SetParent(seatPoint);
        if(destinationUI != null) destinationUI.SetActive(true);
    }

    public void ExitVehicle()
    {
        isSitting = false;
        isMoving = false;
        vrCameraRig.SetParent(null);
        vrCameraRig.position = exitPoint.position;
        vrCameraRig.rotation = Quaternion.identity;
        if(destinationUI != null) destinationUI.SetActive(false);
    }
}