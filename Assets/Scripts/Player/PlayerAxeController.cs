using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAxeController : MonoBehaviour
{
    private Animator _animator;
    private GameObject AxeModel;
    private readonly int _attackID = Animator.StringToHash("Attack");
    private bool isAttacking = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        AxeModel = transform.GetChild(0).gameObject;
    }

    // player input callback
    void OnAxeAttack(InputValue val)
    {
        if (isAttacking) return;

        isAttacking = true;
        _animator.SetTrigger(_attackID);
        AxeModel.SetActive(true);
    }

    // called from the animation event (clip)
    public void OnStopAttack()
    {
        isAttacking = false;
        AxeModel.SetActive(false);
    }
}
