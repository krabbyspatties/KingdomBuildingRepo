using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 10f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Camera cam;
    private Vector3 lastMousePosition;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = cam.ScreenToWorldPoint(lastMousePosition) 
                          - cam.ScreenToWorldPoint(Input.mousePosition);

            transform.position += delta;
            lastMousePosition = Input.mousePosition;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize,
                minZoom,
                maxZoom
            );
        }
    }
}