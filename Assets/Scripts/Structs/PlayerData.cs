using UnityEngine;

namespace Structs
{
    public struct PlayerData
    {
        public Vector3 LastSeenPosition { get; set; }
        public int ScoreMultiplier { get; set; }

        public PlayerData(Vector3 lastSeenPosition, int scoreMultiplier)
        {
            ScoreMultiplier = scoreMultiplier;
            LastSeenPosition = lastSeenPosition;
        }
    }
}