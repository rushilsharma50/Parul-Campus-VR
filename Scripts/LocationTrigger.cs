using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public IndoorManager manager;
    public StreetViewNode startingRoom;

    // Triggered when the player shoots it with the VR laser pointer
    public void OnVRInteract()
    {
        if (manager != null && startingRoom != null)
        {
            manager.SwitchToIndoor(startingRoom);
        }
    }

    // Triggered if the player physically walks into the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (manager != null && startingRoom != null)
            {
                manager.SwitchToIndoor(startingRoom);
            }
        }
    }
}