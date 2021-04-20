using UnityEngine;

namespace Structs
{
    public class PlayerData
    {
        public Vector3 LastSeenPosition { get; set; }
        public int ScoreMultiplier { get; set; }
        
        public int ClearlyThereStack { get; set; }
        
        public int UpsideDownStack { get; set; }
        public float IntoxicationLevel { get; set; }

        public PlayerData(Vector3 currentPosition)
        {
            LastSeenPosition = currentPosition;
            ScoreMultiplier = 1;
            ClearlyThereStack = 0;
            UpsideDownStack = 0;
            IntoxicationLevel = 0.0f;
        }
    }
}