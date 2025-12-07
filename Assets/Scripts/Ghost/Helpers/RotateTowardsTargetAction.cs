using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateTowardsTarget", story: "Rotate [Self] Towards [Target] upto [AngleThreshold]", category: "Action", id: "bbe2591ba28838a3a1811a9c3179bc05")]
public partial class RotateTowardsTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> AngleThreshold;

    /// <summary>
    /// To rotate the ghost towards the player before attacking
    /// </summary>
    protected override Status OnUpdate()
    {
        // validate target
        if (Target.Value == null)
        {
            Debug.LogWarning("RotateTowardsTarget: No target assigned.");
            return Status.Failure;
        }

        // direction to target
        Vector3 dir = Target.Value.transform.position - Self.Value.transform.position;
        dir.y = 0f; // ignoring vertical rotation

        if (dir.sqrMagnitude < Mathf.Epsilon) return Status.Success;    // already at target position

        // rotate towards target
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        Self.Value.transform.rotation = Quaternion.RotateTowards(Self.Value.transform.rotation, targetRotation, 180 * Time.deltaTime);

        // check if rotation is close enough
        float angle = Quaternion.Angle(Self.Value.transform.rotation, targetRotation);
        if (angle <= AngleThreshold.Value) return Status.Success;

        return Status.Running;
    }
}