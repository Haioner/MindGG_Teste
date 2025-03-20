using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class CameraDragZoom : MonoBehaviour
{
    [Header("Drag")]
    [SerializeField] private bool canDragHorizontal = true;
    [SerializeField] private bool canDragVertical = true;
    [SerializeField] private float baseDragSpeed = 0.25f, deceleration = 15f;
    [SerializeField] private Vector2 minCameraPosition, maxCameraPosition;

    [Header("Zoom")]
    [SerializeField] private bool canZoom = true;
    [SerializeField] private float zoomSpeed = 18f, minZoom = -4f, maxZoom = -60f;
    [SerializeField] private TextMeshProUGUI zoomText;

    public static event System.Action<float> OnZoomChanged;

    private Vector3 lastMousePosition, currentVelocity;
    private Camera mainCamera;
    private float dragSpeed;
    private bool isOrthographic;

    private void Awake()
    {
        mainCamera = Camera.main != null ? Camera.main : mainCamera;
        isOrthographic = mainCamera.orthographic;
        if (canZoom)
            UpdateZoom();
    }

    private void Update()
    {
        HandleDrag();
        ApplyDeceleration();

        if (canZoom)
            HandleZoom();
    }

    public float GetCurrentZoom()
    {
        if (mainCamera != null)
        {
            if (isOrthographic)
            {
                float zoomFactor = Mathf.InverseLerp(maxZoom, minZoom, mainCamera.orthographicSize);
                return Mathf.Lerp(100f, 1f, zoomFactor);
            }
            else
            {
                float zoomFactor = Mathf.InverseLerp(minZoom, maxZoom, mainCamera.transform.position.z);
                return Mathf.Lerp(100f, 1f, zoomFactor);
            }
        }
        return 0f;
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            lastMousePosition = Input.mousePosition;
            currentVelocity = Vector3.zero;
        }
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float deltaX = canDragHorizontal ? -delta.x * dragSpeed * Time.deltaTime : 0;
            float deltaY = canDragVertical ? -delta.y * dragSpeed * Time.deltaTime : 0;
            currentVelocity = new Vector3(deltaX, deltaY, 0);
            mainCamera.transform.position = ClampCamera(mainCamera.transform.position + currentVelocity);
            lastMousePosition = Input.mousePosition;
        }
    }

    private Vector3 ClampCamera(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minCameraPosition.x, maxCameraPosition.x);
        position.y = Mathf.Clamp(position.y, minCameraPosition.y, maxCameraPosition.y);
        return position;
    }

    private void ApplyDeceleration()
    {
        if (!Input.GetMouseButton(0) && currentVelocity.sqrMagnitude > 0.01f * 0.01f)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            mainCamera.transform.position = ClampCamera(mainCamera.transform.position + currentVelocity);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (isOrthographic)
            {
                float newSize = mainCamera.orthographicSize - scroll * zoomSpeed;
                mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            }
            else
            {
                Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * scroll * zoomSpeed;
                newPosition.z = Mathf.Clamp(newPosition.z, maxZoom, minZoom);
                mainCamera.transform.position = newPosition;
            }
            UpdateZoom();
        }
    }

    private void UpdateZoom()
    {
        float zoomFactor = isOrthographic
            ? Mathf.InverseLerp(maxZoom, minZoom, mainCamera.orthographicSize)
            : Mathf.InverseLerp(minZoom, maxZoom, mainCamera.transform.position.z);

        dragSpeed = baseDragSpeed * Mathf.Lerp(2f, 10f, zoomFactor);
        float zoomValue = Mathf.Lerp(100f, 1f, zoomFactor);

        if (zoomText != null)
            zoomText.SetText($"{zoomValue:F0}x");

        OnZoomChanged?.Invoke(zoomValue);
    }
}