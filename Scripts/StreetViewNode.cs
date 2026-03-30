using UnityEngine;

public class StreetViewNode : MonoBehaviour
{
    [Header("Dynamic Texture Loading")]
    [Tooltip("Type the EXACT name of the texture file here, without the .jpg or .png extension")]
    public string textureFileName; 

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