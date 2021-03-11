using UnityEngine;

namespace Code.Scripts
{
    [RequireComponent(typeof(Animator))]
    // Controller class for the player to allow movement of the player model in the demo scene. (Mostly taken from Lab1)
    public class PlayerMovementController : MonoBehaviour
    {
        private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");
        private float velocityX, velocityZ;
        private float acceleration = 1.5f;
        private float decelerationFactor = 1.5f;
        private float maxVelocity = 2.0f;
        private bool moveForward, moveBackward, moveLeft, moveRight, isRunning;
        private GameObject plane;
        private Animator animator;

        private const KeyCode MoveForwardKey = KeyCode.W;
        private const KeyCode MoveBackwardKey = KeyCode.S;
        private const KeyCode RunKey = KeyCode.LeftShift;
        private const KeyCode MoveRightKey = KeyCode.D;
        private const KeyCode MoveLeftKey = KeyCode.A;

        private Vector3 planeDimensions;
        private Vector3 planePosition;
        
        void Awake()
        {
            plane = GameObject.Find("Plane");
            animator = GetComponent<Animator>();
            var planeSize = plane.GetComponent<MeshFilter>().mesh.bounds.size;
            var planeScaling = plane.transform.localScale;
            planeDimensions = new Vector3(
                planeSize.x * planeScaling.x,
                planeSize.y * planeScaling.y,
                planeSize.z * planeScaling.z);
            planePosition = plane.transform.position;
        }

        private void VelocityUpdate()
        {
            // Forward/Backward
            if (moveForward && velocityZ < maxVelocity)
            {
                velocityZ += Time.deltaTime * acceleration;
            } 
            else if (!moveForward && velocityZ > 0.0f)
            {
                velocityZ -= Time.deltaTime * decelerationFactor  * acceleration;
            }
            if (moveBackward && velocityZ > -maxVelocity)
            {
                velocityZ -= Time.deltaTime * acceleration;
            } 
            else if (!moveBackward  && velocityZ < 0.0f)
            {
                velocityZ += Time.deltaTime * decelerationFactor  * acceleration;
            }
            // Left/Right
            if (moveLeft && velocityX > -maxVelocity)
            {
                velocityX -= Time.deltaTime * acceleration;
            }
            else if (!moveLeft && velocityX < 0.0f)
            {
                velocityX += Time.deltaTime * decelerationFactor  * acceleration;
            }
            if (moveRight && velocityX < maxVelocity)
            {
                velocityX += Time.deltaTime * acceleration;
            }
            else if (!moveRight && velocityX > 0.0f)
            {
                velocityX -= Time.deltaTime * decelerationFactor  * acceleration;
            }
        }

        private void ConstrainVelocity()
        {
            if (moveForward && velocityZ > maxVelocity)
            {
                velocityZ = maxVelocity;
            }
            if (moveBackward && velocityZ < -maxVelocity)
            {
                velocityZ = -maxVelocity;
            }
            if (moveLeft && velocityX < -maxVelocity)
            {
                velocityX = -maxVelocity;
            }
            if (moveRight && velocityX > maxVelocity)
            {
                velocityX = maxVelocity;
            }
        }
        
        void Update()
        {
            moveForward = Input.GetKey(MoveForwardKey);
            isRunning = Input.GetKey(RunKey);
            moveLeft = Input.GetKey(MoveLeftKey);
            moveRight = Input.GetKey(MoveRightKey);
            moveBackward = Input.GetKey(MoveBackwardKey);
            VelocityUpdate();
            ConstrainVelocity();
            
            animator.SetFloat(VelocityXHash, velocityX);
            animator.SetFloat(VelocityZHash, velocityZ);
            
            BoundCheck();
        }

        // Places the player on the opposite side of the plane if they try to leave it.
        private void BoundCheck()
        {
            var newPos = new Vector3();
            for (var i = 0; i < 3; i+=2)
            {
                if (transform.position[i] < planePosition[i]-planeDimensions[i]/2.0f)
                {
                    newPos[i] = planePosition[i]+planeDimensions[i]/2.0f;
                } 
                else if (transform.position[i] > planePosition[i] + planeDimensions[i]/2.0f )
                {
                    newPos[i] = planePosition[i]-planeDimensions[i]/2.0f;
                }
                else
                {
                    newPos[i] = transform.position[i];
                }
            }

            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
