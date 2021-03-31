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
        
        /*
         * Since the rigidbody is not directly on the game object that is moving. We cannot use it to get the velocity.
         * The velocity needs to be computed manually.
         */
        private void Update()
        {
            Vector3 currentPosition = transform.position;
            Vector3 velocity = (currentPosition - PlayerData.PreviousPosition) / Time.deltaTime;
            PlayerData.Velocity.Set(velocity.x, velocity.y, velocity.z);
            PlayerData.PreviousPosition.Set(currentPosition.x, currentPosition.y, currentPosition.z);
        }
    }
}