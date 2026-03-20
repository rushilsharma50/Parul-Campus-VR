using UnityEngine;

public class IndoorManager : MonoBehaviour
{
    [Header("VR Components")]
    [Tooltip("The script attached to OVRCameraRig that handles flying.")]
    public MonoBehaviour outdoorFlyCam; // Changed to MonoBehaviour so it accepts any script

    [Tooltip("The GameObject on your Right Controller holding the VRPointer/Ray Interactor.")]
    public GameObject vrPointer;

    [Tooltip("Assign your OVRCameraRig root object here.")]
    public Transform vrCameraRig;

    [Header("Environment Setup")]
    [Tooltip("Parent object of your Cesium/Outdoor world.")]
    public GameObject cesiumWorld;

    [Tooltip("Parent object containing your 360 Sphere and Arrows.")]
    public GameObject indoorUI;

    [Tooltip("Drag your giant 3D Inverted Sphere here.")]
    public MeshRenderer indoorSphereRenderer;

    // Track the current room
    private StreetViewNode currentNode;

    void Awake()
    {
        if (indoorSphereRenderer == null)
        {
            Debug.LogError("IndoorManager: Missing the 'Indoor Sphere Renderer'. Please assign your 3D sphere.");
        }

        // Start in Outdoor Mode
        SwitchToOutdoor();
    }

    // ================= MODE CONTROL =================
    public void SwitchToIndoor(StreetViewNode startNode)
    {
        // 1. Disable Outdoor Movement
        if (outdoorFlyCam != null) outdoorFlyCam.enabled = false;

        // 2. Ensure Pointer is ON (to click navigation arrows)
        if (vrPointer != null) vrPointer.SetActive(true);

        // 3. Swap Environments (This saves massive amounts of RAM!)
        if (cesiumWorld != null) cesiumWorld.SetActive(false);
        if (indoorUI != null) indoorUI.SetActive(true);

        // 4. Teleport Player to Center of the Sphere
        if (vrCameraRig != null)
        {
            vrCameraRig.position = indoorSphereRenderer.transform.position;
        }

        // 5. Load the first room
        EnterNode(startNode);

        Debug.Log("Switched to Indoor Mode");
    }

    public void SwitchToOutdoor()
    {
        // 1. Disable Indoor Node
        if (currentNode != null)
        {
            currentNode.OnNodeExit();
            currentNode.gameObject.SetActive(false);
        }

        // 2. Swap Environments
        if (indoorUI != null) indoorUI.SetActive(false);
        if (cesiumWorld != null) cesiumWorld.SetActive(true);

        // 3. Enable Flying
        if (outdoorFlyCam != null) outdoorFlyCam.enabled = true;

        // 4. Manage Pointer
        if (vrPointer != null) vrPointer.SetActive(true);

        Debug.Log("Switched to Outdoor Mode");
    }

    // ================= NODE CONTROL =================
    public void EnterNode(StreetViewNode newNode)
    {
        if (newNode == null) return;

        // Deactivate previous node
        if (currentNode != null)
        {
            currentNode.OnNodeExit();
            currentNode.gameObject.SetActive(false);
        }

        // Set new node
        currentNode = newNode;
        currentNode.gameObject.SetActive(true);

        // Apply 8K Texture to the URP Unlit Sphere
        if (currentNode.skyboxTexture != null && indoorSphereRenderer != null)
        {
            // CRITICAL FIX: URP Unlit uses "_BaseMap", not "_MainTex"
            indoorSphereRenderer.material.SetTexture("_BaseMap", currentNode.skyboxTexture);
        }
        else
        {
            Debug.LogWarning($"Node '{currentNode.name}' is missing a Skybox Texture!");
        }
     

        // Trigger Node Logic (Show Arrows)
        currentNode.OnNodeEnter();
    }
}