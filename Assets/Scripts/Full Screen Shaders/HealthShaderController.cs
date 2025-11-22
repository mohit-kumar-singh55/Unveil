using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthShaderController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Initial Vignette Intensity")]
    [SerializeField] private float _vignetteIntensity = 1f;     // don't change
    [Tooltip("Initial Vignette Power")]
    [SerializeField] private float _vignettePower = 2f;     // don't change
    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenHealthShader;
    [SerializeField] private Material _material;

    private readonly int _vignetteIntensityID = Shader.PropertyToID("_VignetteIntensity");
    private readonly int _vignettePowerID = Shader.PropertyToID("_VignettePower");

    void Start()
    {
        // setting initial values
        _material.SetFloat(_vignetteIntensityID, _vignetteIntensity);
        _material.SetFloat(_vignettePowerID, _vignettePower);
    }

    public void SetVignette(float power)
    {
        _material.SetFloat(_vignetteIntensityID, _vignetteIntensity + (1 - power));
        _material.SetFloat(_vignettePowerID, power * _vignettePower);
    }
}
