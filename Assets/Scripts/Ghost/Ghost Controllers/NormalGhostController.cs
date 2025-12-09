using System.Collections;
using UnityEngine;

public class NormalGhostController : GhostController
{
    #region Serialized Properties
    [Header("General Settings")]
    [Tooltip("Amount of damage the ghost will give to the player using Headbutt")]
    [SerializeField] private float _giveDamageToPlayer = 10f;

    [Header("Normal Ghost Settings")]
    [Tooltip("Time in seconds after which the ghost will make a duplicate ghost")]
    [SerializeField] private float _makeDuplicateGhostAfterSeconds = 30f;
    [Tooltip("Whether the ghost can make duplicate ghosts")]
    [SerializeField] private bool _canMakeDuplicateGhost = true;
    #endregion

    #region Private Properties
    private GhostsManager _ghostsManager;
    #endregion

    #region Public Properties
    public float DamageToPlayer => _giveDamageToPlayer;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        _ghostsManager = GetComponentInParent<GhostsManager>();
    }

    public override void Activate(bool activate = true)
    {
        base.Activate(activate);

        // make a duplicate ghost, if allowed
        if (_canMakeDuplicateGhost && activate)
        {
            StartCoroutine(MakeDuplicateGhost());
            _canMakeDuplicateGhost = true;      // allowing to make only one duplicate ghost
        }
    }

    IEnumerator MakeDuplicateGhost()
    {
        yield return new WaitForSeconds(_makeDuplicateGhostAfterSeconds);
        _ghostsManager.ActivateAnyGhost();       // not actually instantiating a new ghost, just making any non-active ghost active
    }
}
