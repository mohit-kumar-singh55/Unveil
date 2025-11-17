using UnityEngine;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour
{
    Camera cam;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float currentZoom;
    public float sensitivity = 1;

    FirstPersonMovement character;


    void Awake()
    {
        // Get the cam on this gameObject and the defaultZoom.
        cam = GetComponent<Camera>();
        if (cam)
        {
            defaultFOV = cam.fieldOfView;
        }
    }

    void Start()
    {
        character = GetComponentInParent<FirstPersonMovement>();
    }

    void Update()
    {
        // Update the currentZoom and the cam's fieldOfView.
        currentZoom += character.lookInput.y * sensitivity * .05f;
        currentZoom = Mathf.Clamp01(currentZoom);
        cam.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
    }
}
