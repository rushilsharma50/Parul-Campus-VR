using UnityEngine;

public class VRFlyCam : MonoBehaviour
{
    [Header("Flight Settings")]
    public float flySpeed = 10.0f;
    public float rotationSnapAngle = 45f;
    public float verticalSpeed = 5.0f;

    [Header("References")]
    public Transform cameraRig; // Assign OVRCameraRig here
    public Transform centerEyeAnchor; // Assign CenterEyeAnchor here

    private bool isRotating = false;

    void Update()
    {
        // 1. Movement (Left Thumbstick)
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        // Get the direction the HEAD is looking, but flatten it to the horizon (no flying into the ground)
        Vector3 forwardDir = centerEyeAnchor.forward;
        forwardDir.y = 0; 
        forwardDir.Normalize();

        Vector3 rightDir = centerEyeAnchor.right;
        rightDir.y = 0;
        rightDir.Normalize();

        Vector3 moveDir = (forwardDir * primaryAxis.y + rightDir * primaryAxis.x) * flySpeed * Time.deltaTime;
        cameraRig.Translate(moveDir, Space.World);

        // 2. Vertical Movement (Triggers) - Like an elevator
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) // Left Trigger
        {
            cameraRig.Translate(Vector3.down * verticalSpeed * Time.deltaTime, Space.World);
        }
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) // Right Trigger
        {
            cameraRig.Translate(Vector3.up * verticalSpeed * Time.deltaTime, Space.World);
        }

        // 3. Snap Rotation (Right Thumbstick)
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        
        if (Mathf.Abs(secondaryAxis.x) > 0.5f)
        {
            if (!isRotating)
            {
                float rotateAmount = (secondaryAxis.x > 0) ? rotationSnapAngle : -rotationSnapAngle;
                cameraRig.Rotate(0, rotateAmount, 0);
                isRotating = true; // Lock until stick is released
            }
        }
        else
        {
            isRotating = false;
        }
    }
}