using System.Collections;
using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class GhostController : MonoBehaviour
{
    [Tooltip("Time in seconds after which the ghost will make a duplicate ghost")]
    [SerializeField] private float makeDuplicateGhostAfterSeconds = 30f;

    private GhostsManager ghostsManager;
    private BehaviorGraphAgent behaviorGraphAgent;

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

    IEnumerator MakeDuplicateGhost()
    {
        yield return new WaitForSeconds(makeDuplicateGhostAfterSeconds);
        ghostsManager.ActivateAnyGhost();       // not actually instantiating a new ghost, just making any non-active ghost active
    }
}
