using UnityEngine;

public class TourManager : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public string roomName;         // Name (e.g. "Lobby")
        public Material skyboxImage;    // The 360 Material for this room
        public GameObject arrowContainer; // The generic GameObject holding arrows for this room
    }

    public Room[] allRooms; // List of your 5 rooms

    void Start()
    {
        // Start at the first room in the list (Element 0)
        LoadRoom(0);
    }

    public void LoadRoom(int index)
    {
        // 1. Turn OFF all arrow containers (hide everything)
        foreach (var room in allRooms)
        {
            if (room.arrowContainer != null)
                room.arrowContainer.SetActive(false);
        }

        // 2. Set the Skybox
        RenderSettings.skybox = allRooms[index].skyboxImage;
        DynamicGI.UpdateEnvironment(); // Fixes lighting

        // 3. Turn ON only the arrows for the current room
        if (allRooms[index].arrowContainer != null)
        {
            allRooms[index].arrowContainer.SetActive(true);
        }
    }
}