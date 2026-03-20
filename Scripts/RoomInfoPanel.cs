using UnityEngine;
using TMPro; // We use TextMeshPro because standard text looks blurry in VR

public class RoomInfoPanel : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The parent object holding the background panel and text.")]
    public GameObject panelVisuals;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private bool isShowing = false;

    void Start()
    {
        // Hide the panel when the game starts
        HidePanel();
    }

    public void UpdateInfo(string title, string description)
    {
        if (titleText != null) titleText.text = title;
        if (descriptionText != null) descriptionText.text = description;
    }

    // This is the method your VR Info Button will call
    public void TogglePanel()
    {
        isShowing = !isShowing;
        if (panelVisuals != null) panelVisuals.SetActive(isShowing);
    }

    public void HidePanel()
    {
        isShowing = false;
        if (panelVisuals != null) panelVisuals.SetActive(false);
    }
}