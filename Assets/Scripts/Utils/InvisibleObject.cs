using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField] Material originalMaterial;     // The material to use when the object become visible

    private MeshRenderer meshRenderer;

    void Awake()
    {
        TryGetComponent(out meshRenderer);
    }

    public void ReplaceMaterialWithOriginal()
    {
        if (meshRenderer == null) return;

        meshRenderer.material = originalMaterial;
    }
}
