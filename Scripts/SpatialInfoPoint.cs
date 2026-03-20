using UnityEngine;
using TMPro;

public class SpatialInfoPoint : MonoBehaviour
{
    [Header("Information Content")]
    public string infoTitle = "Point of Interest";
    [TextArea(3, 10)]
    public string infoDescription = "Enter the specific details about this area or object here.";

    [Header("UI References")]
    [Tooltip("The World Space Canvas attached to this specific point.")]
    public GameObject uiCanvas;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private Transform playerCamera;

    void Start()
    {
        // Find the VR headset camera automatically
        if (Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        // Apply the text
        if (titleText != null) titleText.text = infoTitle;
        if (descriptionText != null) descriptionText.text = infoDescription;

        // Hide the panel when the scene starts
        if (uiCanvas != null) uiCanvas.SetActive(false);
    }

    // Call this from your XR Simple Interactable
    public void OnVRInteract()
    {
        if (uiCanvas == null) return;

        // Toggle the panel on or off
        bool isShowing = !uiCanvas.activeSelf;
        uiCanvas.SetActive(isShowing);

        // If we just turned it on, force it to face the player so it's easy to read
        if (isShowing && playerCamera != null)
        {
            uiCanvas.transform.LookAt(playerCamera);
            // Fix for Unity UI: Canvases render backwards when using LookAt, so we flip it
            uiCanvas.transform.forward = -uiCanvas.transform.forward;
        }
    }
}