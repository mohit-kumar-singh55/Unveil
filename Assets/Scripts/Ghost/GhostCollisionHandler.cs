using UnityEngine;

public class GhostCollisionHandler : MonoBehaviour
{
    private GhostController ghostController;

    void Start()
    {
        ghostController = GetComponentInParent<GhostController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // checking if being attacked by player axe
        if (collision.gameObject.CompareTag(TAGS.AXE))
        {
            ghostController.OnHitByPlayerAxe(collision.GetContact(0).point - collision.transform.position);
        }
    }
}
