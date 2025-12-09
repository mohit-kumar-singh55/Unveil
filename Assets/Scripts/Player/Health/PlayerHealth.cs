using Unity.Cinemachine;
using UnityEngine;

// Instead of showing health bar, strengthen the vegnette type shader
[RequireComponent(typeof(HealthShaderController), typeof(DamageShaderController))]
public class PlayerHealth : Health
{
    #region Serialized Properties
    [Tooltip("How long to wait before taking next damage")]
    [SerializeField] private float _takeNextDamageAfter = 0.5f;
    #endregion

    #region Private Properties
    private float _timeSinceLastDamage = 0f;
    private HealthShaderController _healthShader;
    private DamageShaderController _damageShader;
    private CinemachineImpulseSource _impulseSource;
    #endregion

    void Awake()
    {
        _impulseSource = TryGetComponent(out CinemachineImpulseSource impulseSource) ? impulseSource : null;

        if (!_impulseSource)
        {
            Debug.LogError("Impulse source not found on the player.");
            enabled = false;
        }
    }

    void Start()
    {
        _healthShader = GetComponent<HealthShaderController>();
        _damageShader = GetComponent<DamageShaderController>();
    }

    void Update()
    {
        if (_timeSinceLastDamage < _takeNextDamageAfter) _timeSinceLastDamage += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.CompareTag(TAGS.GHOST))
        {
            // check if enough time since last damage
            if (_timeSinceLastDamage < _takeNextDamageAfter) return;

            if (!collision.transform.parent.TryGetComponent(out NormalGhostController ghostController)) return;

            // check if ghost attacking
            if (ghostController.IsAttacking) TakeDamage(ghostController.DamageToPlayer);
        }
    }

    protected override void TakeDamage(float damageAmount = 10f)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damageAmount, 0, MaxHealth);

        // strengthen the vegnette
        _healthShader.SetVignette(CurrentHealth / MaxHealth);

        // display damage effect
        _damageShader.ShowDamageEffect();

        // camera shake
        _impulseSource.GenerateImpulse();

        // reset timer
        _timeSinceLastDamage = 0f;

        // TODO: play hurt sound

        // TODO: game over if current health <= 0
    }
}
