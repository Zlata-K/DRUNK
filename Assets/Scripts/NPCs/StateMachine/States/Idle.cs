using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    public Idle()
    {
        name = "Idle";
    }

    public override void Move(GameObject player, GameObject npc)
    {
        //No movement
    }
}
