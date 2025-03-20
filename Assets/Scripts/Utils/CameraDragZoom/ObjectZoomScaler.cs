using UnityEngine;

public class ObjectZoomScaler : MonoBehaviour
{
    [SerializeField] private float minScale = 0.005f;
    [SerializeField] private float maxScale = 0.04f;
    private Transform cachedTransform;

    private void Awake() => cachedTransform = GetComponent<RectTransform>() ?? GetComponent<Transform>();
    private void OnEnable() => CameraDragZoom.OnZoomChanged += AdjustScale;
    private void OnDisable() => CameraDragZoom.OnZoomChanged -= AdjustScale;

    private void Start()
    {
        var screenDrag = FindFirstObjectByType<CameraDragZoom>();
        AdjustScale(screenDrag.GetCurrentZoom());
    }

    private void AdjustScale(float zoomValue)
    {
        float scale = Mathf.Lerp(maxScale, minScale, Mathf.InverseLerp(1f, 100f, zoomValue));
        cachedTransform.localScale = new Vector3(scale, scale, 1f);
    }
}