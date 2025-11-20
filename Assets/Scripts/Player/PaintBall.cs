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

    // ** painting splash decal **
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LAYER.PAINTABLE))
        {
            // collision point
            ContactPoint contact = collision.contacts[0];

            // check if it's a ghost (collider is on the child object but tag is on the parent)
            bool isGhost = collision.gameObject.CompareTag(TAGS.GHOST);

            // if it's a ghost, activate it
            if (isGhost) collision.gameObject.GetComponentInParent<GhostController>().Activate();

            // show the decal
            PlaceSplashDecal(contact.point, contact.normal, collision.gameObject, isGhost);
        }
    }

    private void PlaceSplashDecal(Vector3 point, Vector3 normal, GameObject collidedWith, bool isGhost = false)
    {
        Vector3 pos = point + normal * 0.01f;       // with offset so it doesn't overlap
        // rotating towards the normal
        Quaternion rot = Quaternion.LookRotation(-normal);      // "-" to flip the normal, otherwise it will project inside the object

        // TODO: replace with a pool
        DecalRenderingLayer decalRenderingLayer = Instantiate(paintSplashDecal, pos, rot, collidedWith.transform).GetComponent<DecalRenderingLayer>();    // parented to the collided object so it moves with object

        // change rendering layer for ghost decals
        if (isGhost) decalRenderingLayer.ChangeRenderingLayer();

        // replace material if invisible object
        if (collidedWith.TryGetComponent(out InvisibleObject invisibleObject))
        {
            invisibleObject.ReplaceMaterialWithOriginal();
            // TODO: instead of just replacing material, show some reveal animation using shaders
        }

        Destroy(gameObject);
    }
}
