using UnityEngine;

/// <summary>
/// Controls the spectator camera navigation using RTS-style mechanics.
/// Manages planar movement via keyboard axis inputs, mouse-drag rotation 
/// via the Right Mouse Button, and scroll-wheel zooming with altitude constraints.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>The pacing speed at which the camera pans across the map space via keyboard inputs.</summary>
    public float moveSpeed = 15f;

    /// <summary>The speed multiplier applied to scroll wheel inputs for tactical camera zooming.</summary>
    public float scrollSpeed = 500f;

    /// <summary>The sensitivity rotation speed multiplier utilized during right-click panning drags.</summary>
    public float rotationSpeed = 250f;

    /// <summary>The lowest boundary value allowed for the camera altitude Y-coordinate.</summary>
    public float minHeight = 3f;

    /// <summary>The highest boundary value allowed for the camera altitude Y-coordinate.</summary>
    public float maxHeight = 25f;

    /// <summary>Internal Euler tracker caching current pitch rotation value across the X axis.</summary>
    private float rotationX;

    /// <summary>Internal Euler tracker caching current yaw rotation value across the Y axis.</summary>
    private float rotationY;

    /// <summary>
    /// Standard Unity callback. Caches the initial Euler angles of the camera viewport structure to initialize tracking loops.
    /// </summary>
    void Start()
    {
        Vector3 startRotation = transform.eulerAngles;
        rotationX = startRotation.x;
        rotationY = startRotation.y;
    }

    /// <summary>
    /// Standard Unity callback. Executes orientation look checks (RMB), processes layout keyboard movement translations, 
    /// increments zoom tracking vectors, and forces viewport altitude constraints.
    /// </summary>
    void Update()
    {
        // 1. Viewport Look Rotation (Right Mouse Button Drag loop)
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;

            // Constrain pitch angle boundaries to prevent the camera from turning upside down
            rotationX = Mathf.Clamp(rotationX, 10f, 85f);

            transform.eulerAngles = new Vector3(rotationX, rotationY, 0f);
        }

        // 2. Keyboard Planar Pan Movement
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Flatten forward directional vector to keep translations parallel to the grid layout floor plane
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        // Flatten right directional vector to keep translations parallel to the grid layout floor plane
        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 movement = (forward * inputZ) + (right * inputX);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // 3. Scroll Wheel Zoom Interaction
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            transform.position += transform.forward * scroll * scrollSpeed * Time.deltaTime;
        }

        // 4. Altitude Bounds Enforcement
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minHeight, maxHeight);
        transform.position = clampedPosition;
    }
}