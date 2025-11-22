using UnityEngine;

[RequireComponent(typeof(HealthShaderController), typeof(DamageShaderController))]

// Instead of showing health bar, strengthen the vegnette type shader
public class PlayerHealth : Health
{
    [Tooltip("How long to wait before taking next damage")]
    [SerializeField] private float _takeNextDamageAfter = 0.5f;

    private float _timeSinceLastDamage = 0f;
    private HealthShaderController _healthShader;
    private DamageShaderController _damageShader;

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

            if (!collision.transform.parent.TryGetComponent(out GhostController ghostController)) return;

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

        // reset timer
        _timeSinceLastDamage = 0f;

        // TODO: play hurt sound

        // TODO: game over if current health <= 0
    }
}
