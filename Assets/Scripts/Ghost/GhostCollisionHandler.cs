using UnityEngine;

public class GhostCollisionHandler : MonoBehaviour
{
    private GhostController ghostController;

    void Start()
    {
        ghostController = GetComponentInParent<GhostController>();
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     // checking if being attacked by player axe
    //     if (other.transform.parent != null && other.transform.parent.CompareTag(TAGS.AXE))
    //     {
    //         ghostController.OnHitByPlayerAxe();
    //     }
    // }

    void OnCollisionEnter(Collision collision)
    {
        // checking if being attacked by player axe
        if (collision.transform.parent != null && collision.transform.parent.CompareTag(TAGS.AXE))
        {
            Debug.Log("Ghost hit by axe via OnCollisionEnter");
            ghostController.OnHitByPlayerAxe(collision.GetContact(0).point - collision.transform.position);
        }
    }
}
