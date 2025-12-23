using UnityEngine;

public class StreetViewNode : MonoBehaviour
{
    [Header("Data")]
    public Texture skyboxTexture; // The 360 image for THIS spot
    
    [Header("Connections")]
    // Drag the arrow objects that belong to this spot here.
    // When we enter this node, we turn these ON. When we leave, we turn them OFF.
    public GameObject[] navigationArrows; 

    public void OnEnterNode()
    {
        // 1. Tell the Manager to change the Skybox to MY texture
        IndoorManager manager = FindObjectOfType<IndoorManager>();
        manager.ChangeSkyboxTexture(skyboxTexture);

        // 2. Enable my arrows so the player can see where to go next
        foreach (GameObject arrow in navigationArrows)
        {
            arrow.SetActive(true);
        }
    }

    public void OnExitNode()
    {
        // Disable my arrows so they don't float in the next room
        foreach (GameObject arrow in navigationArrows)
        {
            arrow.SetActive(false);
        }
    }
}