using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected string name;

    public abstract void Move(GameObject player, GameObject npc);

    public string Name => name;
}
