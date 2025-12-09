using UnityEngine;

public class ChestCollisionHandler : MonoBehaviour
{
    private bool _bossGhostAlive = true;

    void OnEnable()
    {
        BossGhostController.OnBossGhostDeath += () => _bossGhostAlive = false;
    }

    void OnDisable()
    {
        BossGhostController.OnBossGhostDeath -= () => _bossGhostAlive = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(TAGS.PLAYER)) return;

        // if boss ghost is alive, game is not over yet
        if (_bossGhostAlive)
        {
            // TODO: show ui notification that, need to defeat boss ghost first to take the treasure
            Debug.Log("Need to defeat boss ghost first to take the treasure");
            return;
        }

        // TODO: show ui notification that, treasure has been taken
        Debug.Log("Treasure has been taken");
    }
}
