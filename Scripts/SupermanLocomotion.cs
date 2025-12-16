using UnityEngine;
using UnityEngine.XR;

public class SupermanLocomotion : MonoBehaviour
{
    public float flySpeed = 10.0f;
    public float rotationSpeed = 45.0f;
    
    // Assign these in Inspector (Standard Quest Inputs)
    private InputDevice targetDevice;

    void Update()
    {
        // Get Left Controller (Movement)
        var leftHandDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        
        if (leftHandDevices.Count > 0)
        {
            InputDevice device = leftHandDevices[0];
            Vector2 primary2DValue;

            // FLYING (Left Stick)
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DValue))
            {
                Vector3 forward = Camera.main.transform.forward;
                Vector3 right = Camera.main.transform.right;

                // Move in the direction you are looking
                Vector3 moveDir = (forward * primary2DValue.y + right * primary2DValue.x).normalized;
                transform.position += moveDir * flySpeed * Time.deltaTime;
            }
        }

        // Get Right Controller (Vertical / Rotation)
        var rightHandDevices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        if (rightHandDevices.Count > 0)
        {
            InputDevice device = rightHandDevices[0];
            Vector2 primary2DValue;
            
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DValue))
            {
                // UP / DOWN (Right Stick Y-Axis)
                transform.position += Vector3.up * primary2DValue.y * flySpeed * Time.deltaTime;

                // ROTATE (Right Stick X-Axis)
                transform.Rotate(0, primary2DValue.x * rotationSpeed * Time.deltaTime, 0);
            }
        }
    }
}