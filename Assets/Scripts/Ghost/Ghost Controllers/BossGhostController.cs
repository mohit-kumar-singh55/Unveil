using UnityEngine;

public class BossGhostController : GhostController
{
    #region Serialized Properties

    #endregion

    protected override void Start()
    {
        base.Start();

        // activating boss ghost on load
        Activate();
    }
}
