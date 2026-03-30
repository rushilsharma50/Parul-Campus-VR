using UnityEngine;
using System.Collections; // REQUIRED for Coroutines (Asynchronous loading)

public class IndoorManager : MonoBehaviour
{
    [Header("VR Components")]
    public MonoBehaviour outdoorFlyCam; 
    public GameObject vrPointer;   
    public Transform vrCameraRig;

    [Header("Environment Setup")]
    public GameObject cesiumWorld;
    public GameObject indoorUI;
    public MeshRenderer indoorSphereRenderer;

    private StreetViewNode currentNode;
    private Texture currentLoadedTexture; 

    void Awake()
    {
        if (indoorSphereRenderer == null) Debug.LogError("Missing Indoor Sphere!");
        SwitchToOutdoor();
    }

    // ================= MODE CONTROL =================
    public void SwitchToIndoor(StreetViewNode startNode)
    {
        if (outdoorFlyCam != null) outdoorFlyCam.enabled = false;
        if (vrPointer != null) vrPointer.SetActive(true);
        if (cesiumWorld != null) cesiumWorld.SetActive(false);
        if (indoorUI != null) indoorUI.SetActive(true);

        if (vrCameraRig != null)
            vrCameraRig.position = indoorSphereRenderer.transform.position;

        EnterNode(startNode);
    }

    public void SwitchToOutdoor()
    {
        if (currentNode != null)
        {
            currentNode.OnNodeExit();
            currentNode.gameObject.SetActive(false);
        }

        // FLUSH THE VRAM completely when returning outdoors
        if (currentLoadedTexture != null)
        {
            indoorSphereRenderer.material.SetTexture("_BaseMap", null);
            Resources.UnloadAsset(currentLoadedTexture);
            currentLoadedTexture = null;
            Resources.UnloadUnusedAssets(); // Safely clear all memory ghosts
        }

        if (indoorUI != null) indoorUI.SetActive(false);
        if (cesiumWorld != null) cesiumWorld.SetActive(true);
        if (outdoorFlyCam != null) outdoorFlyCam.enabled = true;
        if (vrPointer != null) vrPointer.SetActive(true);
    }

    // ================= NODE CONTROL =================
    public void EnterNode(StreetViewNode newNode)
    {
        if (newNode == null) return;

        // Instead of loading instantly, we start the background Coroutine
        StartCoroutine(LoadNodeAsync(newNode));
    }

    // --- THE ASYNCHRONOUS DYNAMIC LOADING ENGINE ---
    private IEnumerator LoadNodeAsync(StreetViewNode newNode)
    {
        // 1. Deactivate old node
        if (currentNode != null)
        {
            currentNode.OnNodeExit();
            currentNode.gameObject.SetActive(false);
        }

        currentNode = newNode;
        currentNode.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(currentNode.textureFileName))
        {
            // 2. Disconnect the old texture from the sphere material
            indoorSphereRenderer.material.SetTexture("_BaseMap", null);

            // 3. Unload the old texture from the graphics card
            if (currentLoadedTexture != null)
            {
                Resources.UnloadAsset(currentLoadedTexture);
                currentLoadedTexture = null;
            }

            // 4. THE MAGIC CRASH FIX: Force Unity to sweep the RAM and actually delete unused files before loading the next one
            yield return Resources.UnloadUnusedAssets();

            // 5. BACKGROUND LOAD: Ask Unity to load the heavy photo in the background
            ResourceRequest request = Resources.LoadAsync<Texture>(currentNode.textureFileName);
            
            // 6. Pause this script and wait here until the computer successfully finishes loading it
            yield return request;

            currentLoadedTexture = request.asset as Texture;

            // 7. Apply it to the sphere now that it is safely in memory
            if (currentLoadedTexture != null)
            {
                indoorSphereRenderer.material.SetTexture("_BaseMap", currentLoadedTexture);
            }
            else
            {
                Debug.LogError($"Could not find file '{currentNode.textureFileName}' in the Resources folder!");
            }
        }

        currentNode.OnNodeEnter();
    }
}