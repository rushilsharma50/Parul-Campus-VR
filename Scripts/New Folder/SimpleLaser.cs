using UnityEngine;

public class SimpleLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float maxDistance = 10f;
    public LayerMask layerMask; // Set this to "Default" for now

    void Update()
    {
        // 1. Draw the Laser
        RaycastHit hit;
        Vector3 endPosition = transform.position + (transform.forward * maxDistance);
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask))
        {
            // If we hit something, stop the laser at that point
            endPosition = hit.point;

            // 2. Check for Click (Trigger Button)
            // "SecondaryIndexTrigger" is the Right Controller Trigger
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetMouseButtonDown(0))
            {
                // Check if the thing we hit is an Arrow
                TeleportArrow arrow = hit.collider.GetComponent<TeleportArrow>();
                if (arrow != null)
                {
                    arrow.Activate(); // CLICK IT!
                }
            }
        }

        // Update the Line Visuals
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }
}