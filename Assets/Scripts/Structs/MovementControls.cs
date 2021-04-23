using UnityEngine;

namespace Structs
{
    public struct MovementControls
    {
        public KeyCode MoveForwardKey { get; set; }
        public KeyCode MoveBackwardKey { get; set; }
        public KeyCode MoveRightKey { get; set; }
        public KeyCode MoveLeftKey { get; set; }

        public MovementControls(KeyCode forward, KeyCode backward, KeyCode right, KeyCode left)
        {
            MoveForwardKey = forward;
            MoveBackwardKey = backward;
            MoveRightKey = right;
            MoveLeftKey = left;
        }

        public void InverseKeys()
        {
            var tempF = MoveForwardKey;
            MoveForwardKey = MoveBackwardKey;
            MoveBackwardKey = tempF;

            var tempR = MoveRightKey;
            MoveRightKey = MoveLeftKey;
            MoveLeftKey = tempR;
        }
    }
}