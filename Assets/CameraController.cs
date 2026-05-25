using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float scrollSpeed = 500f;
    public float rotationSpeed = 250f;

    public float minHeight = 3f;
    public float maxHeight = 25f;

    private float rotationX;
    private float rotationY;

    void Start()
    {
        Vector3 startRotation = transform.eulerAngles;
        rotationX = startRotation.x;
        rotationY = startRotation.y;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;

            rotationX = Mathf.Clamp(rotationX, 10f, 85f);

            transform.eulerAngles = new Vector3(rotationX, rotationY, 0f);
        }

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 movement = (forward * inputZ) + (right * inputX);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            transform.position += transform.forward * scroll * scrollSpeed * Time.deltaTime;
        }

        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minHeight, maxHeight);
        transform.position = clampedPosition;
    }
}