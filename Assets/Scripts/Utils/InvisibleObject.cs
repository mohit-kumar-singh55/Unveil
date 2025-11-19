using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [Tooltip("オブジェクトを見えない状態にするために使用するマテリアル")]
    [SerializeField] private Material invisibleMaterial;       // The material to use to make object invisible

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;       // The material to use when the object become visible

    void Awake()
    {
        if (TryGetComponent(out meshRenderer))
        {
            originalMaterials = meshRenderer.materials;
            meshRenderer.materials = new Material[] { invisibleMaterial };
        }
    }

    public void ReplaceMaterialWithOriginal()
    {
        if (meshRenderer == null) return;

        meshRenderer.materials = originalMaterials;
    }
}
