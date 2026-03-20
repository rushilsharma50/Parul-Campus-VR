using UnityEngine;

public class StreetViewNode : MonoBehaviour
{
    [Header("Skybox")]
    public Texture skyboxTexture;

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