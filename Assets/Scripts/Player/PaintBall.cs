using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]
public class PaintBall : MonoBehaviour
{
    [SerializeField] private GameObject paintSplashDecal;

    public void SetDecalMaterial(Material material)
    {
        if (paintSplashDecal == null) return;

        if (paintSplashDecal.TryGetComponent(out DecalProjector decalProjector))
            decalProjector.material = material;
    }

    // painting splash decal
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LAYER.PAINTABLE))
        {
            ContactPoint contact = collision.contacts[0];

            Vector3 pos = contact.point + contact.normal * 0.01f;       // with offset so it doesn't overlap
            // rotating towards the normal
            Quaternion rot = Quaternion.LookRotation(-contact.normal);      // "-" to flip the normal, otherwise it will project inside the object

            // TODO: replace with a pool
            Instantiate(paintSplashDecal, pos, rot);

            // replace material if invisible object
            if (collision.gameObject.TryGetComponent(out InvisibleObject invisibleObject))
            {
                invisibleObject.ReplaceMaterialWithOriginal();
                // TODO: instead of just replacing material, show some reveal animation using shaders
            }

            Destroy(gameObject);
        }
    }
}
