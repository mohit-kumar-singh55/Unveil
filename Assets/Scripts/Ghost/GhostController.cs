using System.Collections;
using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class GhostController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Time in seconds after which the ghost will make a duplicate ghost")]
    [SerializeField] private float _makeDuplicateGhostAfterSeconds = 30f;
    [Tooltip("Amount of damage the ghost will give to the player")]
    [SerializeField] private float _giveDamageToPlayer = 10f;
    [Header("Got Hit Settings")]
    [Tooltip("No. of times the ghost can be hit before it is deactivated")]
    [SerializeField] private int _noOfHitsCanBeTaken = 3;
    [SerializeField] private Material _damageTakenMaterial;

    private bool _isAttacking = false;
    private int _noOfHitsTaken = 0;
    private GhostsManager _ghostsManager;
    private BehaviorGraphAgent _behaviorGraphAgent;
    private MeshRenderer _ghostMeshRenderer;

    public bool IsAttacking => _isAttacking;
    public float DamageToPlayer => _giveDamageToPlayer;

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
        // need this to change the material to show hit
        _ghostMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    /// <summary>
    /// Activates or deactivates the ghost's AI and makes a duplicate ghost
    /// </summary>
    public void Activate(bool activate = true)
    {
        _behaviorGraphAgent.enabled = activate;

        // make a duplicate ghost
        if (activate) StartCoroutine(MakeDuplicateGhost());
        else StopAllCoroutines();
    }

    // it will be called from the animation event (clip)
    public void SetIsAttacking() => _isAttacking = !_isAttacking;

    public void OnHitByPlayerAxe()
    {
        // TODO: show hit animation and play sfx
        if (_noOfHitsTaken >= _noOfHitsCanBeTaken) return;

        _noOfHitsTaken++;
        transform.localScale *= 0.8f;       // reduce the ghost's scale

        // replacing 1st matrial with damage material for sometime
        Material originalMaterial = _ghostMeshRenderer.material;
        _ghostMeshRenderer.material = _damageTakenMaterial;

        StartCoroutine(ReplaceMaterialWithOriginal(.2f, originalMaterial));

        // deactivate the ghost
        if (_noOfHitsTaken >= _noOfHitsCanBeTaken)
        {
            Activate(false);
            GetComponentInChildren<Collider>().enabled = false;
        }
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
}