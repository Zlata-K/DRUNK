using UnityEngine;

public abstract class State
{

    protected NPCManager _npcManager;
    
    public abstract void Move();

    protected void LookWhereYouAreGoing(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _npcManager.transform.rotation = Quaternion.RotateTowards(_npcManager.transform.rotation, lookRotation, NPCsGlobalVariables.MaxAngleChange * Time.deltaTime);
    }
    
}
