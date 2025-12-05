using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class DecalRenderingLayer : MonoBehaviour
{
    [Tooltip("Rendering layer to use for ghost decals")]
    [SerializeField] private RenderingLayerMask renderingLayer;

    private DecalProjector decalProjector;

    void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    /// <summary>
    /// Changes the rendering layer of the DecalProjector
    /// for ghost decals
    /// </summary>
    public void ChangeRenderingLayer()
    {
        decalProjector.renderingLayerMask = renderingLayer;
    }
}
