using Structs;
using UnityEngine;

namespace Player
{
    public class PlayerDataManager : MonoBehaviour
    {
        public PlayerData PlayerData { get; set; }

        private void Start()
        {
            PlayerData = new PlayerData(gameObject.transform.position);
        }

        private void Update()
        {
            if (PlayerData.ClearlyThereStack == 0) {
                PlayerData.LastSeenPosition = transform.position;
            }

        }
    }
}