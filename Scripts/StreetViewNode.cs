using UnityEngine;

public class StreetViewNode : MonoBehaviour
{
    [Header("Skybox")]
    public Texture skyboxTexture; // 360 panoramic image for this room

    [Header("Navigation Arrows")]
    public GameObject[] arrows;

    public void OnNodeEnter()
    {
        foreach (var arrow in arrows)
            arrow.SetActive(true);
    }

    public void OnNodeExit()
    {
        foreach (var arrow in arrows)
            arrow.SetActive(false);
    }
}
