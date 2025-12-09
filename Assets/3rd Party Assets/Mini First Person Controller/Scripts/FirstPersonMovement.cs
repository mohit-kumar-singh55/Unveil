using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;

    private Vector2 moveInput;
    [HideInInspector] public Vector2 lookInput;

    bool sprintPressed = false;
    [HideInInspector] public bool jumpPressed = false;

    bool freezePlayer = false;
    Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new();

    void OnEnable()
    {
        BossFinalAttack.OnBossFinalAttackStart += OnBossAttackStart;
    }

    void OnDisable()
    {
        BossFinalAttack.OnBossFinalAttackStart -= OnBossAttackStart;
    }

    void Awake()
    {
        // Get the rb on this.
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (freezePlayer) return;

        // Update IsRunning from input.
        IsRunning = canRun && sprintPressed;

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[^1]();
        }

        // Get targetVelocity from input.
        // Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        Vector2 targetVelocity = new(moveInput.x * targetMovingSpeed, moveInput.y * targetMovingSpeed);

        // Apply movement.
        rb.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.y);
    }

    // player input callbacks
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        sprintPressed = value.Get<float>() == 1f;
    }

    public void OnJump(InputValue value)
    {
        jumpPressed = true;
    }

    void OnBossAttackStart(Transform ghostBossTransform)
    {
        freezePlayer = true;

        // if not do this, player will keep rotating
        rb.isKinematic = true;

        // rotate towards the ghost boss to show attack animation
        StartCoroutine(RotateTowardsTarget(ghostBossTransform, .5f, 1f));
    }

    IEnumerator RotateTowardsTarget(Transform target, float duration, float angleThreshold = 1f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        float angle = Quaternion.Angle(transform.localRotation, targetRotation);
        while (angle > angleThreshold)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, 180 * Time.deltaTime / duration);
            angle = Quaternion.Angle(transform.localRotation, targetRotation);
            yield return null;
        }
    }
}