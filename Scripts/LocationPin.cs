using UnityEngine;

public class LocationPin : MonoBehaviour
{
    public IndoorManager manager;
    public StreetViewNode startingRoom;

    // We changed this to OnVRInteract so your VRPointer can find it!
    public void OnVRInteract()
    {
        if (manager != null && startingRoom != null)
        {
            manager.SwitchToIndoor(startingRoom);
        }
        else
            Debug.LogError("LocationPin is missing the Manager or the Starting Room!");
    }
}