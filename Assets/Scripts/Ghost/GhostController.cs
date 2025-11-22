using System.Collections;
using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class GhostController : MonoBehaviour
{
    [Tooltip("Time in seconds after which the ghost will make a duplicate ghost")]
    [SerializeField] private float _makeDuplicateGhostAfterSeconds = 30f;
    [Tooltip("Amount of damage the ghost will give to the player")]
    [SerializeField] private float _giveDamageToPlayer = 10f;

    private bool isAttacking = false;
    private GhostsManager ghostsManager;
    private BehaviorGraphAgent behaviorGraphAgent;

    public bool IsAttacking => isAttacking;
    public float DamageToPlayer => _giveDamageToPlayer;

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Awake()
    {
        ghostsManager = GetComponentInParent<GhostsManager>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    /// <summary>
    /// Activates or deactivates the ghost's AI and makes a duplicate ghost
    /// </summary>
    public void Activate(bool activate = true)
    {
        behaviorGraphAgent.enabled = activate;

        // make a duplicate ghost
        if (activate) StartCoroutine(MakeDuplicateGhost());
        else StopAllCoroutines();
    }

    // it will be called from the animation evnet marker of animation clip
    public void SetIsAttacking() => isAttacking = !isAttacking;

    IEnumerator MakeDuplicateGhost()
    {
        yield return new WaitForSeconds(_makeDuplicateGhostAfterSeconds);
        ghostsManager.ActivateAnyGhost();       // not actually instantiating a new ghost, just making any non-active ghost active
    }
}
