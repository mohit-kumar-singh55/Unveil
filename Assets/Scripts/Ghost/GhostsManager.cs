using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

// place this script on the root object of list of ghosts
public class GhostsManager : MonoBehaviour
{
    [SerializeField] private string ghostStopWorkingVariableName = "StopWorking";

    private List<BehaviorGraphAgent> ghostAgents = new();

    void Start()
    {
        // get all ghost agents
        gameObject.GetComponentsInChildren(true, ghostAgents);
    }

    /// <summary>
    /// stop all ghosts from working
    /// </summary>
    public void StopAllGhosts()
    {
        foreach (BehaviorGraphAgent ghostAgent in ghostAgents)
        {
            // ignore non-active ghosts
            if (!ghostAgent.enabled) return;

            // stop ghost from working
            ghostAgent.SetVariableValue(ghostStopWorkingVariableName, true);
        }
    }
}
