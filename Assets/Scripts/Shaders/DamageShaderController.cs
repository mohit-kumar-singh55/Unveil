using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageShaderController : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float _damageDisplayTime = 1f;
    [SerializeField] private float _damageFadeOutTime = .5f;

    [Header("Settings")]
    [Tooltip("Initial Vignette Intensity")]
    [SerializeField] private float _vignetteIntensity = 1f;     // dont't change
    [Tooltip("Initial Voronoi Intensity")]
    [SerializeField] private float _voronoiIntensity = 1f;      // dont't change

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _fullScreenDamageShader;
    [SerializeField] private Material _material;

    private readonly int _vignetteIntensityID = Shader.PropertyToID("_VignetteIntensity");
    private readonly int _voronoiIntensityID = Shader.PropertyToID("_VoronoiIntensity");

    void Start()
    {
        _fullScreenDamageShader.SetActive(false);
    }

    public void ShowDamageEffect() => StartCoroutine(DisplayDamage());

    private IEnumerator DisplayDamage()
    {
        // show immediately
        _fullScreenDamageShader.SetActive(true);
        _material.SetFloat(_vignetteIntensityID, _vignetteIntensity);
        _material.SetFloat(_voronoiIntensityID, _voronoiIntensity);

        yield return new WaitForSeconds(_damageDisplayTime);

        // fade out
        float elapsedTime = 0;
        while (elapsedTime < _damageFadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            _material.SetFloat(_vignetteIntensityID, Mathf.Lerp(_vignetteIntensity, 0, elapsedTime / _damageFadeOutTime));
            _material.SetFloat(_voronoiIntensityID, Mathf.Lerp(_voronoiIntensity, 0, elapsedTime / _damageFadeOutTime));
            yield return null;
        }

        _fullScreenDamageShader.SetActive(false);
    }
}
