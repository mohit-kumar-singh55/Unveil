using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAxeController : MonoBehaviour
{
    private Animator _animator;
    private GameObject AxeModel;
    private readonly int _attack1ID = Animator.StringToHash("Attack1");
    private readonly int _attack2ID = Animator.StringToHash("Attack2");
    private readonly int _attack3ID = Animator.StringToHash("Attack3");
    private bool _isAttacking = false;
    private int _noOfTimesPressed = 0;
    private float _lastPressedTime = 0f;

    private const int NO_OF_ATTACKS_AVAILABLE = 3;  // total no. of attacks implemented in the game
    private const float MAX_TIME_TO_WAIT_BETWEEN_INPUTS = 0.25f;    // wait for inputs only upto this time

    void Awake()
    {
        _animator = GetComponent<Animator>();
        AxeModel = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (_isAttacking || _noOfTimesPressed <= 0) return;

        if (_lastPressedTime < MAX_TIME_TO_WAIT_BETWEEN_INPUTS && _noOfTimesPressed < NO_OF_ATTACKS_AVAILABLE) _lastPressedTime += Time.deltaTime;
        else if (_lastPressedTime >= MAX_TIME_TO_WAIT_BETWEEN_INPUTS || _noOfTimesPressed >= NO_OF_ATTACKS_AVAILABLE)
        {
            _isAttacking = true;

            Debug.Log("Player attacked with axe " + _noOfTimesPressed + " times.");

            switch (_noOfTimesPressed)
            {
                case 1:
                    AxeAttackOneAndTwo(_attack1ID);
                    break;
                case 2:
                    AxeAttackOneAndTwo(_attack2ID);
                    break;
                case 3:
                    AxeAttackOneAndTwo(_attack3ID);
                    break;
                default:
                    AxeAttackOneAndTwo(_attack1ID);
                    break;
            }

            // _isAttacking = false;

            // reset values and call attack funcs
            _noOfTimesPressed = 0;
            _lastPressedTime = 0f;
        }
    }

    // player input callback
    private void OnAxeAttack(InputValue val)
    {
        if (_isAttacking) return;

        // _isAttacking = true;

        _lastPressedTime = 0f;
        _noOfTimesPressed = Mathf.Clamp(_noOfTimesPressed + 1, 0, NO_OF_ATTACKS_AVAILABLE);
    }

    // called from the animation event (clip)
    public void OnStopAttack()
    {
        _isAttacking = false;
        AxeModel.SetActive(false);
    }

    private void AxeAttackOneAndTwo(int attackID)
    {
        _animator.SetTrigger(attackID);
        AxeModel.SetActive(true);
    }
}
