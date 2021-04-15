using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public Idle(NPCManager npcManager)
    {
        _npcManager = npcManager;
    }

    public override void Move()
    {
        //No movement
    }
}
