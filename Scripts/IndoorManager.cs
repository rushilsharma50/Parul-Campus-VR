using UnityEngine;
using System.Collections.Generic;

public class IndoorManager : MonoBehaviour
{
    [Header("Skybox Settings")]
    public Material indoorSkyboxMaterial; // Drag 'Skybox_Indoor' here
    public Material outdoorSkyboxMaterial; // Drag the default Cesium/Skybox here
    public Material baseSkyboxMaterial; // Drag your skybox material here as a reference
    
    private Material skyboxMaterialInstance;

    [Header("Rooms")]
    public List<Texture> roomTextures; // (Optional now) Keep empty if using Nodes
    private int currentRoomIndex = 0;

    [Header("Mode Control")]
    public GameObject outdoorCam; // Your Superman/Simulator Camera
    public GameObject indoorCam;  // Drag 'IndoorCamera' here
    public GameObject cesiumWorld; // Drag 'CesiumGeoreference' here
    public GameObject uiCanvas;    // Drag 'Canvas' (Exit Button) here
    public GameObject tourParent;
    private Material skyboxMaterial;

    void Start()
    {
        // Start in Outdoor Mode by default
        //SwitchToOutdoor();
        // 1. Force the tour object to be active
        if (tourParent != null) 
        {
            tourParent.SetActive(true);
        }
    }
    void Awake()
    {
        // Create an instance of the skybox material to avoid modifying the asset
        if (RenderSettings.skybox != null)
        {
            skyboxMaterialInstance = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skyboxMaterialInstance;
        }
        else if (baseSkyboxMaterial != null)
        {
            skyboxMaterialInstance = new Material(baseSkyboxMaterial);
            RenderSettings.skybox = skyboxMaterialInstance;
        }
        else
        {
            Debug.LogError("IndoorManager: No skybox material found! Please assign a base skybox material.");
        }
    }

    void Update()
    {
        // Press 'M' to Toggle Modes (Debug)
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (outdoorCam.activeSelf) SwitchToIndoor();
            else SwitchToOutdoor();
        }
    }

    public void SwitchToIndoor()
    {
        // 1. Disable Outdoor stuff
        outdoorCam.SetActive(false);
        cesiumWorld.SetActive(false); 

        // 2. Enable Indoor stuff
        indoorCam.SetActive(true);
        
        // 3. Show the Exit Button
        if (uiCanvas != null) uiCanvas.SetActive(true);

        // 4. Set Skybox Material
        RenderSettings.skybox = indoorSkyboxMaterial;
        
        // NOTE: We do NOT load a texture here anymore. 
        // We wait for the Node (StreetViewNode) to tell us which texture to load.
        
        Debug.Log("Switched to Indoor Mode");
    }

    public void SwitchToOutdoor()
    {
        // 1. Enable Outdoor stuff
        outdoorCam.SetActive(true);
        cesiumWorld.SetActive(true);

        // 2. Disable Indoor stuff
        indoorCam.SetActive(false);
        
        // 3. Hide the Exit Button
        if (uiCanvas != null) uiCanvas.SetActive(false);

        // 4. Restore Skybox
        RenderSettings.skybox = outdoorSkyboxMaterial;
        Debug.Log("Switched to Outdoor Mode");
    }

    // This is called by your Nodes (StreetViewNode script)
    public void ChangeSkyboxTexture(Texture newTexture)
    {
        if (newTexture == null)
        {
            Debug.LogError("IndoorManager: Attempted to change skybox to null texture!");
            return;
        }
        
        if (skyboxMaterialInstance == null)
        {
            Debug.LogError("IndoorManager: Skybox material instance is null!");
            return;
        }
        
        // Set the texture based on the shader type
        string shaderName = skyboxMaterialInstance.shader.name;
        
        if (shaderName.Contains("Panoramic"))
        {
            skyboxMaterialInstance.SetTexture("_MainTex", newTexture);
        }
        else if (shaderName.Contains("Cubemap"))
        {
            skyboxMaterialInstance.SetTexture("_Tex", newTexture);
        }
        else if (shaderName.Contains("6 Sided"))
        {
            // For 6-sided, you'd need to set all 6 textures
            skyboxMaterialInstance.SetTexture("_FrontTex", newTexture);
            Debug.LogWarning("Using 6-Sided skybox - only setting front texture. You may need to set all 6 sides.");
        }
        else
        {
            // Fallback - try common property names
            skyboxMaterialInstance.SetTexture("_MainTex", newTexture);
        }
        
        // Force environment update (important for reflections)
        DynamicGI.UpdateEnvironment();
        
        Debug.Log($"Skybox changed to: {newTexture.name}");
    }
    void OnDestroy()
    {
        // Clean up the material instance when destroyed
        if (skyboxMaterialInstance != null)
        {
            Destroy(skyboxMaterialInstance);
        }
    }
    
    // Helper for the Red Pin (LocationTrigger)
    public void SetRoomIndex(int index)
    {
        currentRoomIndex = index;
    }
}