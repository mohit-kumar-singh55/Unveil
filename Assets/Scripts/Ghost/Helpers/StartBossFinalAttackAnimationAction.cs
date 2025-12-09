using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Start Boss Final Attack Animation", story: "Call PlayFinalAttackAnimation function from [BossFinalAttack]", category: "Action", id: "31a5625c578d3c3f93dbfbfed07f2c0f")]
public partial class StartBossFinalAttackAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFinalAttack> BossFinalAttack;

    protected override Status OnStart()
    {
        BossFinalAttack.Value.PlayFinalAttackAnimation();
        return Status.Success;
    }
}

