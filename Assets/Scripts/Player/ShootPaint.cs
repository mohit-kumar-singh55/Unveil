using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPaint : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject paintBall;

    // callback called from input system
    private void OnShoot(InputValue inputVal)
    {
        Rigidbody rb = Instantiate(paintBall, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * speed, ForceMode.Impulse);
    }
}
