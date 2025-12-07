using UnityEngine;
using UnityEngine.SceneManagement;

public class InvisibleDoorCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // if not collided with player or is still invisible, return
        if (!collision.gameObject.CompareTag(TAGS.PLAYER)) return;
        if (!gameObject.TryGetComponent(out InvisibleObject invisibleObject)) return;
        if (!invisibleObject.IsVisible) return;

        // TODO: blacken the screen, play door opening sound, load final level scene
        Debug.Log("Collision with invisible door");

        SceneManager.LoadScene(SCENES.FINAL_LEVEL_SCENE);
    }
}
