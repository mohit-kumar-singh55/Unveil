using UnityEngine;

public class GhostCollisionHandler : MonoBehaviour
{
    private GhostController ghostController;

    void Start()
    {
        ghostController = GetComponentInParent<GhostController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // checking if being attacked by player axe
        if (other.transform.parent != null && other.transform.parent.CompareTag(TAGS.AXE))
        {
            ghostController.OnHitByPlayerAxe();
        }
    }
}
