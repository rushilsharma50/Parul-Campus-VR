using UnityEngine;

public class LocationPin : MonoBehaviour
{
    public StreetViewNode entryNode; // First room of that building

    private IndoorManager manager;

    void Start()
    {
        manager = FindObjectOfType<IndoorManager>();
    }

    public void OnVRInteract() 
{
    if (entryNode == null) return;
    manager.SwitchToIndoor();
    manager.EnterNode(entryNode);
}
}
