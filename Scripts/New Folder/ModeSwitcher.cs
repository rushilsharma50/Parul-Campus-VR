using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
    [Header("Mode Settings")]
    public GameObject campus3DModel;   // Your 3D University Model
    public GameObject interiorUI;      // The Canvas or Arrows for the inside view
    
    [Header("Player Settings")]
    public GameObject locomotionSystem; // The "Locomotion" Setup from Meta Building Blocks

    [Header("First Room")]
    public Material lobbySkybox;       // The 360 image of the first room (Lobby)
    public Material defaultSkybox;     // The default skybox (Sky/Sun) for outside

    // Call this when player clicks the "Door" of the 3D building
    public void EnterBuilding()
    {
        // 1. Hide the 3D Campus (Optional, improves performance)
        campus3DModel.SetActive(false);

        // 2. Disable Walking (Turn off Locomotion)
        // We do this so they don't accidentally teleport while looking at the 360 photo
        if(locomotionSystem != null) locomotionSystem.SetActive(false);

        // 3. Switch to Skybox View
        RenderSettings.skybox = lobbySkybox;
        DynamicGI.UpdateEnvironment();

        // 4. Show Interior Controls (Arrows)
        interiorUI.SetActive(true);
    }

    // Call this to leave the building
    public void ExitBuilding()
    {
        // 1. Show the 3D Campus
        campus3DModel.SetActive(true);

        // 2. Re-enable Walking
        if(locomotionSystem != null) locomotionSystem.SetActive(true);

        // 3. Reset Skybox to Day/Sun
        RenderSettings.skybox = defaultSkybox;
        DynamicGI.UpdateEnvironment();

        // 4. Hide Interior Controls
        interiorUI.SetActive(false);
    }
}