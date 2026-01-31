using UnityEngine;

public class VRPointer : MonoBehaviour
{
    public float rayLength = 50f;
    public LayerMask interactableLayer; // Set this to "Default" or create an "Interactable" layer
    public LineRenderer laserLine;

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        // Draw Laser
        laserLine.SetPosition(0, transform.position);

        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            laserLine.SetPosition(1, hit.point);

            // Check for Click (A Button or Right Trigger)
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.One))
            {
                // Try to find one of your existing components
                if (hit.collider.TryGetComponent(out LocationTrigger locTrigger))
                {
                    locTrigger.OnVRInteract();
                }
                else if (hit.collider.TryGetComponent(out NavigationArrow navArrow))
                {
                    navArrow.OnVRInteract();
                }
                else if (hit.collider.TryGetComponent(out LocationPin locPin))
                {
                    locPin.OnVRInteract();
                }
            }
        }
        else
        {
            laserLine.SetPosition(1, transform.position + (transform.forward * rayLength));
        }
    }
}