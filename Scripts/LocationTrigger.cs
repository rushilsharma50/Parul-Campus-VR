using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    [Header("Configuration")]
    public int roomIndexToLoad = 0; // Which 360 image does this pin belong to?
    public string locationName = "CV Raman Lab"; // Just for debug/UI

    private IndoorManager gameManager;
    public StreetViewNode entryNode;

    void Start()
    {
        // Find the manager automatically so you don't have to drag it every time
        gameManager = FindObjectOfType<IndoorManager>();
    }

    // This function detects mouse clicks (PC) or VR Laser clicks (later)
    void OnMouseDown()
    {
        Debug.Log("Clicked on: " + locationName);
        EnterBuilding();
    }

    public void EnterBuilding()
    {
        gameManager.SwitchToIndoor(); // Switch cameras
        entryNode.OnEnterNode();      // Load the specific first room of this building
    }
}