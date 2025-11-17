using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody rb;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;

    FirstPersonMovement character;

    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Start()
    {
        character = GetComponentInParent<FirstPersonMovement>();
    }

    void Awake()
    {
        // Get rb.
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // Jump when the Jump button is pressed and we are on the ground.
        if (character.jumpPressed && (!groundCheck || groundCheck.isGrounded))
        {
            rb.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
            character.jumpPressed = false;
        }
    }
}
