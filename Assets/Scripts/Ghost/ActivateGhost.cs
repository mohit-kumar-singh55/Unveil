using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class ActivateGhost : MonoBehaviour
{
    private BehaviorGraphAgent behaviorGraphAgent;

    void Awake()
    {
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }

    public void Activate(bool activate = true) => behaviorGraphAgent.enabled = activate;
}
