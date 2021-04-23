using UnityEngine;

namespace Structs
{
    public class PlayerData
    {
        private float _intoxicationLevel;
        private int _scoreMultiplier;
        public Vector3 LastSeenPosition { get; set; }
        
        
        public int ClearlyThereStack { get; set; }
        
        public int UpsideDownStack { get; set; }
        public bool IsKnockedOut { get; set; }
        public int CurrentScore { get; set; }
        public int ScoreMultiplier 
        {
            get => _scoreMultiplier;
            set => _scoreMultiplier = Mathf.Max(value, 0);
        }
        public float IntoxicationLevel
        {
            get => _intoxicationLevel;
            set => _intoxicationLevel = Mathf.Clamp(value, 0.0f, 1.0f);
        }

        
        public PlayerData(Vector3 currentPosition)
        {
            LastSeenPosition = currentPosition;
            ScoreMultiplier = 1;
            ClearlyThereStack = 0;
            UpsideDownStack = 0;
            IntoxicationLevel = 0.0f;
            CurrentScore = 0;
            IsKnockedOut = false;
        }
    }
}