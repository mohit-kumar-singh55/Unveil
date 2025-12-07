using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BehaviorGraphAgent), typeof(NavMeshAgent))]
public class GhostController : MonoBehaviour
{
    #region Serialized Properties
    [Header("General Settings")]
    [Tooltip("Amount of damage the ghost will give to the player using Headbutt")]
    [SerializeField] private float _giveDamageToPlayer = 10f;

    [Header("Got Hit Settings")]
    [Tooltip("No. of times the ghost can be hit before it is deactivated")]
    [SerializeField] private int _noOfHitsCanBeTaken = 3;
    [Tooltip("Scale of the ghost after being hit in percentage of its original scale")]
    [SerializeField] private float _scaleToBeReducedTo = 0.8f;
    [Tooltip("Force to be applied to the ghost after being hit")]
    [SerializeField] private float _forceToBeApplied = 20f;
    [Tooltip("Time after which the ghost will reactivate itself after being hit")]
    [SerializeField] private float _reactivateGhostAfterSeconds = 4f;
    [SerializeField] private Material _damageTakenMaterial;
    #endregion

    #region Private Properties
    private bool _isAttacking = false;
    private int _noOfHitsTaken = 0;
    private bool _isDead = false;
    private bool _isActivated = false;
    private BehaviorGraphAgent _behaviorGraphAgent;
    private MeshRenderer _ghostMeshRenderer;
    private Collider _ghostTempCollider;    // only used when ghost is hit
    private MeshCollider _ghostMeshCollider;    // original mesh collider of the ghost used for painting
    private NavMeshAgent _navMeshAgent;
    #endregion

    #region Public Properties
    public bool IsDead => _isDead;
    public bool IsAttacking => _isAttacking;
    public float DamageToPlayer => _giveDamageToPlayer;
    #endregion

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected virtual void Awake()
    {
        _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    protected virtual void Start()
    {
        // *** init ***
        // need this to change the material to show hit
        _ghostMeshRenderer = GetComponentInChildren<MeshRenderer>();
        _ghostMeshCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        _ghostTempCollider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Activates or deactivates the ghost's AI and makes a duplicate ghost
    /// </summary>
    public virtual void Activate(bool activate = true)
    {
        // ignore if already activated
        if (_isActivated == activate) return;

        _behaviorGraphAgent.enabled = activate;

        // set "Exclude layers" to Nothing in ghost collider to make it collide with player
        if (activate) _ghostMeshCollider.excludeLayers = 0;
        else StopAllCoroutines();

        // if dead, it will not be activated again by any other ghost while making duplicates
        _isDead = !activate;
        _isActivated = activate;
    }

    // it will be called from the animation event (clip)
    public void SetIsAttacking() => _isAttacking = !_isAttacking;

    public void OnHitByPlayerAxe(Vector3 hitDirection)
    {
        // TODO: show hit animation and play sfx
        if (_noOfHitsTaken >= _noOfHitsCanBeTaken) return;

        _noOfHitsTaken++;
        transform.localScale *= _scaleToBeReducedTo;       // reduce the ghost's scale

        // stopping behavior graph temporarily
        _navMeshAgent.enabled = false;
        _behaviorGraphAgent.enabled = false;

        // changing active collider to use physics
        _ghostTempCollider.isTrigger = false;
        _ghostMeshCollider.convex = true;
        _ghostMeshCollider.isTrigger = true;

        // adding rb to apply force
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForceAtPosition(hitDirection.normalized * _forceToBeApplied, transform.position, ForceMode.Impulse);

        // resetting above values after some delay
        StartCoroutine(ResetValuesAfterDelay(_reactivateGhostAfterSeconds));

        // replacing 1st matrial with damage material for sometime
        Material originalMaterial = _ghostMeshRenderer.material;
        _ghostMeshRenderer.material = _damageTakenMaterial;
        StartCoroutine(ReplaceMaterialWithOriginal(.2f, originalMaterial)); // replace mat with original

        // deactivate the ghost
        if (_noOfHitsTaken >= _noOfHitsCanBeTaken)
        {
            ResetHitValues();
            Activate(false);
            _ghostMeshCollider.enabled = false;
        }
    }

    private void ResetHitValues()
    {
        // reset collider and rigidbody
        _ghostTempCollider.isTrigger = true;
        _ghostMeshCollider.isTrigger = false;
        _ghostMeshCollider.convex = false;
        if (TryGetComponent(out Rigidbody rb)) Destroy(rb); // without removing rb, ghost not gonna collide with paint balls properly

        // enable navmesh agent and behavior graph
        _navMeshAgent.enabled = true;
        _behaviorGraphAgent.enabled = true;
    }

    IEnumerator ReplaceMaterialWithOriginal(float duration, Material originalMaterial)
    {
        yield return new WaitForSeconds(duration);
        _ghostMeshRenderer.material = originalMaterial;
    }

    IEnumerator ResetValuesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetHitValues();
    }
}