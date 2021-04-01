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
    }
}