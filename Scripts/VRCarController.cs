using UnityEngine;
using UnityEngine.InputSystem; // Ensure you have Input System installed

public class VRCarController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    public float brakeForce = 20f;

    [Header("References")]
    public Transform driverSeat; // Empty GameObject where player should sit
    public Rigidbody rb;
    
    private GameObject playerRig;
    private bool isDriving = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        playerRig = GameObject.FindWithTag("Player"); // Ensure your OVRCameraRig has the "Player" tag
    }

    void Update()
    {
        if (!isDriving) return;

        // RIGHT JOYSTICK: Forward/Backward (Primary2DAxis on Right Controller)
        Vector2 rightJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        // LEFT JOYSTICK: Turning (Primary2DAxis on Left Controller)
        Vector2 leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // Movement Logic
        float forwardInput = rightJoystick.y;
        float turnInput = leftJoystick.x;

        // Drive
        if (Mathf.Abs(forwardInput) > 0.1f)
        {
            rb.MovePosition(transform.position + transform.forward * forwardInput * moveSpeed * Time.deltaTime);
        }

        // Turn
        if (Mathf.Abs(turnInput) > 0.1f)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.deltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // BUTTON Y: Brake
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, brakeForce * Time.deltaTime);
        }

        // BUTTON X: Exit
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            ExitCar();
        }
    }

    // This method is called by your Meta Interaction SDK (OnSelect)
    public void EnterCar()
    {
        if (isDriving) return;

        isDriving = true;
        // Parent the player to the car so they move together
        playerRig.transform.SetParent(driverSeat);
        playerRig.transform.localPosition = Vector3.zero;
        playerRig.transform.localRotation = Quaternion.identity;
    }

    void ExitCar()
    {
        isDriving = false;
        playerRig.transform.SetParent(null);
        // Move player slightly outside the car so they don't clip
        playerRig.transform.position += transform.right * 2f; 
    }
}