using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPaint : MonoBehaviour
{
    #region Serialized Properties
    [SerializeField] private float speed = 20f;
    [SerializeField] private float intervalOfFire = .5f;    // seconds
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject paintBall;
    [SerializeField] private Material[] decalMaterials;
    #endregion

    #region Private Properties
    private float timeElapsed;
    #endregion

    void Update()
    {
        if (timeElapsed >= intervalOfFire) return;

        timeElapsed += Time.deltaTime;
    }

    // callback called from input system
    private void OnShoot(InputValue inputVal)
    {
        // checking if enough time has elapsed
        if (timeElapsed < intervalOfFire) return;

        // firing
        GameObject ball = Instantiate(paintBall, shootPoint.position, Quaternion.identity);

        // shooting forward
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * speed, ForceMode.Impulse);

        // setting random decal material
        PaintBall pb = ball.GetComponent<PaintBall>();
        pb.SetDecalMaterial(decalMaterials[Random.Range(0, decalMaterials.Length)]);

        timeElapsed = 0;
    }
}
