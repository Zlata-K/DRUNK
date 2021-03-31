using UnityEngine;

public abstract class State
{
    protected string name;

    public abstract void Move(GameObject player, GameObject npc);

    public string Name => name;
    
    protected void LookWhereYouAreGoing(GameObject npc, Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        npc.transform.rotation = Quaternion.RotateTowards(npc.transform.rotation, lookRotation, NPCsGlobalVariables.MaxAngleChange * Time.deltaTime);
    }
    
}
