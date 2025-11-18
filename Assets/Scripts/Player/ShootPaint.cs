using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPaint : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject paintBall;
    [SerializeField] private Transform splashDecalParent;
    [SerializeField] private Material[] decalMaterials;

    // callback called from input system
    private void OnShoot(InputValue inputVal)
    {
        GameObject ball = Instantiate(paintBall, shootPoint.position, Quaternion.identity);

        // shooting forward
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * speed, ForceMode.Impulse);

        // setting random decal material
        PaintBall pb = ball.GetComponent<PaintBall>();
        pb.SetDecalMaterial(decalMaterials[Random.Range(0, decalMaterials.Length)]);
        pb.splashDecalParent = splashDecalParent;   // passing splash decal parent
    }
}
