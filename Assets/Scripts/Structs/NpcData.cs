using NPCs;
using NPCs.StateMachine;
using UnityEngine;

namespace Structs
{
    public class NpcData
    {
        public NPCStateMachine StateMachine;

        public Vector3 PreviousLocation;
        
        public bool Stuck = false;
        public bool CanChase = true;
        public bool CanPunch;
        public bool LookingForPlayer = false;
        public bool Punching;
    }
}