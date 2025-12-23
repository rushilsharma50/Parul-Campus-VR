using UnityEngine;

public class SimpleFlyCam : MonoBehaviour
{
    [Header("Flight Settings")]
    public float mainSpeed = 50.0f; // Regular speed
    public float shiftAdd = 100.0f; // Sprint speed (Left Shift)
    public float maxShift = 300.0f; // Max sprint speed
    public float camSens = 0.15f;   // Mouse sensitivity

    private Vector3 lastMouse = new Vector3(255, 255, 255);

    void Update()
    {
        // 1. Mouse Look (Turning the head)
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;

        // 2. Keyboard Movement (Flying)
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            float totalRun = transform.position.y * 0.5f; // Fly faster when higher up
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        transform.Translate(p);
    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W)) p_Velocity += new Vector3(0, 0, 1);
        if (Input.GetKey(KeyCode.S)) p_Velocity += new Vector3(0, 0, -1);
        if (Input.GetKey(KeyCode.A)) p_Velocity += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.D)) p_Velocity += new Vector3(1, 0, 0);
        // Up and Down flight
        if (Input.GetKey(KeyCode.E)) p_Velocity += new Vector3(0, 1, 0); // Fly Up
        if (Input.GetKey(KeyCode.Q)) p_Velocity += new Vector3(0, -1, 0); // Fly Down
        return p_Velocity;
    }
}