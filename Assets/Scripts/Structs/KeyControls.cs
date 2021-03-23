using UnityEngine;

namespace Structs
{
    public struct KeyControls
    {
        public KeyCode MoveForwardKey { get; set; }
        public KeyCode MoveBackwardKey { get; set; }
        public KeyCode MoveRightKey { get; set; }
        public KeyCode MoveLeftKey { get; set; }

        public KeyControls(KeyCode forward, KeyCode backward, KeyCode right, KeyCode left)
        {
            MoveForwardKey = forward;
            MoveBackwardKey = backward;
            MoveRightKey = right;
            MoveLeftKey = left;
        }
    }
}