using UnityEngine;

public abstract class State
{
    protected NPCManager _npcManager;
    
    public abstract void Move();
}
