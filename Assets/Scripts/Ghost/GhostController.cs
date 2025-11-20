using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class GhostController : MonoBehaviour
{
    private BehaviorGraphAgent behaviorGraphAgent;

    void Awake()
    {
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    /// <summary>
    /// Activates or deactivates the ghost's AI
    /// </summary>
    public void Activate(bool activate = true) => behaviorGraphAgent.enabled = activate;
}
