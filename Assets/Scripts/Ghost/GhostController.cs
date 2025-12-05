using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BehaviorGraphAgent), typeof(NavMeshAgent))]
public class GhostController : MonoBehaviour
{
    #region Serialized Properties
    [Header("Settings")]
    [Tooltip("Time in seconds after which the ghost will make a duplicate ghost")]
    [SerializeField] private float _makeDuplicateGhostAfterSeconds = 30f;
    [Tooltip("Amount of damage the ghost will give to the player")]
    [SerializeField] private float _giveDamageToPlayer = 10f;
    [Header("Got Hit Settings")]
    [Tooltip("No. of times the ghost can be hit before it is deactivated")]
    [SerializeField] private int _noOfHitsCanBeTaken = 3;
    [SerializeField] private Material _damageTakenMaterial;
    #endregion

    #region Private Properties
    private bool _isAttacking = false;
    private int _noOfHitsTaken = 0;
    private bool _isDead = false;
    private GhostsManager _ghostsManager;
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

    void Awake()
    {
        _ghostsManager = GetComponentInParent<GhostsManager>();
        _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    void Start()
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
    public void Activate(bool activate = true)
    {
        _behaviorGraphAgent.enabled = activate;

        if (activate)
        {
            // set "Exclude layers" to Nothing in ghost collider to make it collide with player
            _ghostMeshCollider.excludeLayers = 0;

            // make a duplicate ghost
            StartCoroutine(MakeDuplicateGhost());
        }
        else StopAllCoroutines();

        // if dead, it will not be activated again by any other ghost while making duplicates
        _isDead = !activate;
    }

    // it will be called from the animation event (clip)
    public void SetIsAttacking() => _isAttacking = !_isAttacking;

    public void OnHitByPlayerAxe(Vector3 hitDirection)
    {
        // TODO: show hit animation and play sfx
        if (_noOfHitsTaken >= _noOfHitsCanBeTaken) return;

        _noOfHitsTaken++;
        transform.localScale *= 0.8f;       // reduce the ghost's scale

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
        rb.AddForceAtPosition(hitDirection.normalized * 20f, transform.position, ForceMode.Impulse);

        // resetting above values after some delay
        StartCoroutine(ResetValuesAfterDelay(4f));

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

    IEnumerator MakeDuplicateGhost()
    {
        yield return new WaitForSeconds(_makeDuplicateGhostAfterSeconds);
        _ghostsManager.ActivateAnyGhost();       // not actually instantiating a new ghost, just making any non-active ghost active
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