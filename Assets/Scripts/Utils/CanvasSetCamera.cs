using UnityEngine;

public class CanvasSetCamera : MonoBehaviour
{
    [SerializeField] private string cameraTag = "MainCamera";

    private Canvas _canvas;
    private Camera _camera;

    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag(cameraTag).GetComponent<Camera>();
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = _camera;
    }
}
