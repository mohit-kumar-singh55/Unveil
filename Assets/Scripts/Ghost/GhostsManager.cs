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

            // disable ghost controller to stop making more duplicates
            ghostAgent.gameObject.GetComponent<GhostController>().enabled = false;
        }
    }

    /// <summary>
    /// activate any non-active ghost
    /// </summary>
    public void ActivateAnyGhost()
    {
        BehaviorGraphAgent nonActiveGhost = ghostAgents.Find(ghostAgent => !ghostAgent.enabled);

        if (nonActiveGhost != null)
        {
            // activate ghost
            nonActiveGhost.enabled = true;

            // set "Exclude layers" to Nothing in ghost collider to make it collide with player
            nonActiveGhost.gameObject.GetComponentInChildren<Collider>().excludeLayers = 0;

            // replacing material with original material
            nonActiveGhost.gameObject.GetComponentInChildren<InvisibleObject>().ReplaceMaterialWithOriginal();
        }
    }
}
