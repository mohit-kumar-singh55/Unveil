using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAxeController : MonoBehaviour
{
    #region Serialized Properties
    [Tooltip("Force with which the axe is thrown in 3rd attack.")]
    [SerializeField] private float _axeThrowForce = 50f;
    [Tooltip("Starting point from where the axe is thrown and will return to.")]
    [SerializeField] private Transform _startingPoint;
    #endregion

    #region Private Properties
    private Animator _animator;
    private GameObject AxeModel;
    private Camera _cam;
    private Rigidbody _axeRigidbody;
    private MeshCollider _axeCollider;
    private CinemachineImpulseSource _impulseSource;
    private readonly int _attack1ID = Animator.StringToHash("Attack1");
    private readonly int _attack2ID = Animator.StringToHash("Attack2");
    private bool _isAttacking = false;
    private int _noOfTimesPressed = 0;
    private float _lastPressedTime = 0f;
    #endregion

    #region Constants
    private const int NO_OF_ATTACKS_AVAILABLE = 3;  // total no. of attacks implemented in the game
    private const float MAX_TIME_TO_WAIT_BETWEEN_INPUTS = 0.25f;    // wait for inputs only upto this time
    #endregion

    void Awake()
    {
        _cam = Camera.main;
        _animator = GetComponent<Animator>();
        _impulseSource = TryGetComponent(out CinemachineImpulseSource impulseSource) ? impulseSource : null;

        if (!_startingPoint || !_impulseSource)
        {
            Debug.LogError("Impulse source not found on the Axe model or Starting point not assigned.");
            enabled = false;
        }
    }

    void Start()
    {
        AxeModel = transform.GetChild(0).gameObject;
        _axeRigidbody = AxeModel.TryGetComponent(out Rigidbody rb) ? rb : null;
        _axeCollider = AxeModel.TryGetComponent(out MeshCollider col) ? col : null;

        if (!_axeRigidbody || !_axeCollider)
        {
            Debug.LogError("Rigidbody or Axe mesh collider not found on the Axe model.");
            enabled = false;
        }
    }

    void Update()
    {
        // if button not pressed or already attacking, return
        if (_isAttacking || _noOfTimesPressed <= 0) return;

        // wait for inputs
        if (_lastPressedTime < MAX_TIME_TO_WAIT_BETWEEN_INPUTS && _noOfTimesPressed < NO_OF_ATTACKS_AVAILABLE) _lastPressedTime += Time.deltaTime;
        // if enough time has elapsed, start attack
        else if (_lastPressedTime >= MAX_TIME_TO_WAIT_BETWEEN_INPUTS || _noOfTimesPressed >= NO_OF_ATTACKS_AVAILABLE)
        {
            StartAxeAttack(_noOfTimesPressed);

            // reset values
            _noOfTimesPressed = 0;
            _lastPressedTime = 0f;
        }
    }

    // player input callback
    private void OnAxeAttack(InputValue val)
    {
        if (_isAttacking) return;

        _lastPressedTime = 0f;
        _noOfTimesPressed = Mathf.Clamp(_noOfTimesPressed + 1, 0, NO_OF_ATTACKS_AVAILABLE);
    }

    // called from the animation event (clip) for 1st and 2nd attack
    public void OnStopAttack()
    {
        _isAttacking = false;
        AxeModel.SetActive(false);
    }

    private void StartAxeAttack(int attack_no)
    {
        _isAttacking = true;

        switch (attack_no)
        {
            case 1:
                AxeAttackOneAndTwo(_attack1ID);
                break;
            case 2:
                AxeAttackOneAndTwo(_attack2ID);
                break;
            case 3:
                AxeAttackThree();
                break;
            default:
                AxeAttackThree();
                break;
        }

        // camera shake
        _impulseSource.GenerateImpulse();
    }

    private void AxeAttackOneAndTwo(int attackID)
    {
        _animator.SetTrigger(attackID);
        AxeModel.SetActive(true);
    }

    private void AxeAttackThree()
    {
        // detach the axe model from the axe root
        AxeModel.transform.parent = null;

        AxeModel.SetActive(true);
        _animator.enabled = false;  // rigidbody not gonna work

        // setting starting position and rotation of the axe
        AxeModel.transform.position = _startingPoint.position;
        AxeModel.transform.localScale = Vector3.one;

        // throw the axe (only model not the root object) in the forward direction of the camera
        Vector3 throwDirection = _cam.transform.forward.normalized;
        _axeRigidbody.AddForce(throwDirection * _axeThrowForce, ForceMode.Impulse);
        _axeRigidbody.AddTorque(AxeModel.transform.right * 5f, ForceMode.Impulse);

        // reset axe after some time
        StartCoroutine(ResetThridAttack());
    }

    IEnumerator ResetThridAttack(float delay = 2f)
    {
        yield return new WaitForSeconds(delay);

        // stopping axe movement
        _axeRigidbody.linearVelocity = Vector3.zero;

        // reset scale (because scale changes when detaching and attaching to the axe root)
        AxeModel.transform.localScale = Vector3.one;

        // to avoid any collision interference during way back to the starting position
        float previousMass = _axeRigidbody.mass;
        _axeRigidbody.mass = 0f;
        _axeCollider.enabled = false;

        // move upto starting point
        while (Vector3.Distance(AxeModel.transform.position, _startingPoint.position) > 0.1f)
        {
            AxeModel.transform.position = Vector3.MoveTowards(AxeModel.transform.position, _startingPoint.position, 50f * Time.deltaTime);
            yield return null;
        }

        _impulseSource.GenerateImpulse();

        // reattach the axe model to the axe root
        AxeModel.transform.parent = gameObject.transform;
        AxeModel.transform.rotation = _startingPoint.rotation;

        // reactivate the collision
        _axeRigidbody.mass = previousMass;
        _axeCollider.enabled = true;

        // reactivate animations
        _animator.enabled = true;
        OnStopAttack();
    }
}
