using UnityEngine;

public class TeleportArrow : MonoBehaviour
{
    public LocationNode targetNode; // Where does this go?

    // Called by our SimpleLaser script
    public void Activate()
    {
        FindObjectOfType<StreetViewManager>().LoadNode(targetNode);
    }
}