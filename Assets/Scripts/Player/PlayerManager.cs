using Structs;
using UnityEngine;

namespace Player
{
    public class PlayerManager: MonoBehaviour
    {
        public PlayerData PlayerData { get; set; }

        private void Awake()
        {
            PlayerData = new PlayerData(Vector3.zero, 1);
        }
        
    }
}