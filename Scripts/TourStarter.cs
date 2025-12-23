using UnityEngine;

public class TourStarter : MonoBehaviour
{
    public StreetViewNode firstNode; // Drag Node_01 here in Inspector

    void Start()
    {
        if (firstNode != null)
        {
            firstNode.OnEnterNode(); // This forces the skybox to update immediately
        }
    }
}