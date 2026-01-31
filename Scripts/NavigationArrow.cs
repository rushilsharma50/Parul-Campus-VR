using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    // The variable is named 'targetNode', so we must use this name below
    public StreetViewNode targetNode; 
    private IndoorManager manager;

    void Start()
    {
        manager = FindObjectOfType<IndoorManager>();
    }

    public void OnVRInteract() 
    {
        // FIX: Changed 'entryNode' to 'targetNode'
        if (targetNode == null) 
        {
            Debug.LogError($"Arrow {name} has no Target Node assigned!");
            return;
        }

        // We don't need SwitchToIndoor() here because if you can see 
        // the arrow, you are ALREADY indoors.
        manager.EnterNode(targetNode);
    }
}