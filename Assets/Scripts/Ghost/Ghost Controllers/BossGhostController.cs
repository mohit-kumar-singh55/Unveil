using System;

public class BossGhostController : GhostController
{
    public static event Action OnBossGhostDeath = delegate { };

    protected override void Start()
    {
        base.Start();

        // activating boss ghost on load
        Activate();
    }

    public override void Activate(bool activate = true)
    {
        base.Activate(activate);

        // boss ghost died
        if (!activate) OnBossGhostDeath?.Invoke();
    }
}
