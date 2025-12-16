using UnityEngine;

public class LocationNode : MonoBehaviour
{
    public Material skyboxMaterial;
    public GameObject arrowContainer;

    void Awake()
    {
        // Hide arrows at start so they don't clutter the scene
        if (arrowContainer != null) arrowContainer.SetActive(false);
    }
}