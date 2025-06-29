using UnityEngine;

[ExecuteAlways]
public class AutoScaleBackground : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromCamera = 15f;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        AdjustBackground();
    }

    void Update()
    {
        AdjustBackground();
    }

    void AdjustBackground()
    {
        if (mainCamera == null) return;

        float fov = mainCamera.fieldOfView;
        float aspect = mainCamera.aspect;

        float height = 2f * distanceFromCamera * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float width = height * aspect;

        transform.localScale = new Vector3(width, height, 1f); 

        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

        transform.rotation = Quaternion.identity;
    }
}
