using UnityEngine;

public class IndoorManager : MonoBehaviour
{
    [Header("VR Components")]
    [Tooltip("The script attached to OVRCameraRig that handles flying.")]
    public VRFlyCam outdoorFlyCam; 
    
    [Tooltip("The GameObject on your Right Controller holding the VRPointer script.")]
    public GameObject vrPointer;   

    [Tooltip("Assign your OVRCameraRig root object here.")]
    public Transform vrCameraRig;

    [Header("Skybox Materials")]
    public Material outdoorSkyboxMaterial;     // The default sky/Cesium sky
    public Material indoorPanoramicMaterial;   // The material applied to the inverted sphere/skybox
    
    // We create an instance of the material at runtime so we don't modify the original asset
    private Material indoorRuntimeSkybox;

    [Header("Environment Roots")]
    [Tooltip("Parent object of your Cesium/Outdoor world.")]
    public GameObject cesiumWorld;
    
    [Tooltip("Parent object of your Indoor bubbles/UI.")]
    public GameObject indoorUI;

    // Track the current room
    private StreetViewNode currentNode;

    void Awake()
    {
        // Create a runtime copy of the material to swap textures dynamically
        if (indoorPanoramicMaterial != null)
        {
            indoorRuntimeSkybox = new Material(indoorPanoramicMaterial);
        }
        else
        {
            Debug.LogError("IndoorManager: Missing 'Indoor Panoramic Material'. Please assign it in Inspector.");
        }

        // Start in Outdoor Mode
        ForceOutdoorState();
    }
    // ================= MODE CONTROL =================
    public void SwitchToIndoor()
    {
        // 1. Disable Outdoor Movement
        if (outdoorFlyCam != null) outdoorFlyCam.enabled = false;

        // 2. Ensure Pointer is ON (to click navigation arrows)
        if (vrPointer != null) vrPointer.SetActive(true);

        // 3. Swap Environments
        if (cesiumWorld != null) cesiumWorld.SetActive(false);
        if (indoorUI != null) indoorUI.SetActive(true);

        // 4. Update Skybox to Indoor Material
        RenderSettings.skybox = indoorRuntimeSkybox;
        DynamicGI.UpdateEnvironment(); // Force lighting update

        // 5. Teleport Player to Center
        // We move the Rig to 0,0,0 because that is where we assume the Indoor Sphere is centered.
        if (vrCameraRig != null)
        {
            vrCameraRig.position = Vector3.zero;
            // Note: We generally do NOT reset rotation in VR, as it can cause nausea. 
            // Let the user keep their head orientation.
        }
        // 6. Activate Current Node if exists
        if (currentNode != null)
        {
            currentNode.gameObject.SetActive(true);
            currentNode.OnNodeEnter(); // Ensure arrows are visible
        }

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

        // 3. Restore Outdoor Skybox
        RenderSettings.skybox = outdoorSkyboxMaterial;
        DynamicGI.UpdateEnvironment();

        // 4. Enable Flying
        if (outdoorFlyCam != null) outdoorFlyCam.enabled = true;

        // 5. Manage Pointer
        // KEEP the pointer active so you can click the building entrance (LocationTrigger) again.
        if (vrPointer != null) vrPointer.SetActive(true);

        Debug.Log("Switched to Outdoor Mode");
    }
    // ================= NODE CONTROL =================
    public void EnterNode(StreetViewNode newNode)
    {
        if (newNode == null)
        {
            Debug.LogError("IndoorManager: Attempted to enter a NULL node.");
            return;
        }

        // Deactivate previous node
        if (currentNode != null)
        {
            currentNode.OnNodeExit();
            currentNode.gameObject.SetActive(false);
        }

        // Set new node
        currentNode = newNode;
        currentNode.gameObject.SetActive(true);

        // Apply Texture to Skybox/Sphere
        if (currentNode.skyboxTexture != null)
        {
            // "_MainTex" is the standard property name. 
            // If you use a custom shader, check the property name (e.g., "_Tex", "_BaseMap").
            indoorRuntimeSkybox.SetTexture("_MainTex", currentNode.skyboxTexture);
        }
        else
        {
            Debug.LogWarning($"Node '{currentNode.name}' is missing a Skybox Texture!");
        }

        // Trigger Node Logic (Show Arrows)
        currentNode.OnNodeEnter();
    }

    // ================= SAFETY / INIT =================

    private void ForceOutdoorState()
    {
        // Reset to a known clean state
        if (indoorUI != null) indoorUI.SetActive(false);
        if (cesiumWorld != null) cesiumWorld.SetActive(true);
        
        RenderSettings.skybox = outdoorSkyboxMaterial;
        DynamicGI.UpdateEnvironment();

        if (outdoorFlyCam != null) outdoorFlyCam.enabled = true;
    }
}