using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossFinalAttack : MonoBehaviour
{
    #region Private Properties
    private Animator _animator;
    private Collider _collider;
    private MeshCollider _meshCollider;
    private NavMeshAgent _navMeshAgent;
    private BehaviorGraphAgent _behaviorGraphAgent;
    #endregion

    // trigger name
    private const string BOSS_FINAL_ATTACK = "BossFinalAttack";

    public static event System.Action<Transform> OnBossFinalAttackStart = delegate { };

    void Awake()
    {
        _meshCollider = GetComponentInChildren<MeshCollider>();

        _animator = TryGetComponent(out Animator animator) ? animator : null;
        _collider = TryGetComponent(out Collider collider) ? collider : null;
        _navMeshAgent = TryGetComponent(out NavMeshAgent navMeshAgent) ? navMeshAgent : null;
        _behaviorGraphAgent = TryGetComponent(out BehaviorGraphAgent behaviorGraphAgent) ? behaviorGraphAgent : null;

        if (!_meshCollider || !_animator || !_collider || !_navMeshAgent || !_behaviorGraphAgent)
        {
            Debug.LogError("MeshCollider, Animator, Collider, NavMeshAgent or BehaviorGraphAgent not found on the Ghost.");
            enabled = false;
        }
    }

    // will be called from the behaviour graph
    public void PlayFinalAttackAnimation()
    {
        _collider.enabled = false;
        _navMeshAgent.enabled = false;
        _behaviorGraphAgent.enabled = false;
        _animator.SetTrigger(BOSS_FINAL_ATTACK);

        OnBossFinalAttackStart.Invoke(transform);
    }

    // will be called from the animation clip
    public void OnFinalAttackAnimationEnd() => SceneManager.LoadScene(SCENES.FINAL_LEVEL_DEATH_SCENE);
}
