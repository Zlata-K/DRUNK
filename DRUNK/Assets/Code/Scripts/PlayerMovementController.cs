using UnityEngine;

namespace Code.Scripts
{
    [RequireComponent(typeof(Animator))]
    // Controller class for the player to allow movement of the player model in the demo scene. (Mostly taken from Lab1)
    public class PlayerMovementController : MonoBehaviour
    {
        public float acceleration = 1.5f;
        public float decelerationFactor = 1.5f;
        public float maxVelocity = 2.0f;
        
        private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");
        
        private float _velocityX, _velocityZ;
        private bool _moveForward, _moveBackward, _moveLeft, _moveRight;
        private GameObject _plane;
        private Animator _animator;

        private const KeyCode MoveForwardKey = KeyCode.W;
        private const KeyCode MoveBackwardKey = KeyCode.S;
        private const KeyCode MoveRightKey = KeyCode.D;
        private const KeyCode MoveLeftKey = KeyCode.A;

        private Vector3 _planeDimensions;
        private Vector3 _planePosition;
        
        void Awake()
        {
            _plane = GameObject.Find("Plane");
            _animator = GetComponent<Animator>();
            var planeSize = _plane.GetComponent<MeshFilter>().mesh.bounds.size;
            var planeScaling = _plane.transform.localScale;
            _planeDimensions = new Vector3(
                planeSize.x * planeScaling.x,
                planeSize.y * planeScaling.y,
                planeSize.z * planeScaling.z);
            _planePosition = _plane.transform.position;
        }

        private void VelocityUpdate()
        {
            // Forward/Backward update
            if (_moveForward && _velocityZ < maxVelocity)
            {
                _velocityZ += Time.deltaTime * acceleration;
            } 
            else if (!_moveForward && _velocityZ > 0.0f)
            {
                _velocityZ -= Time.deltaTime * decelerationFactor  * acceleration;
            }
            if (_moveBackward && _velocityZ > -maxVelocity)
            {
                _velocityZ -= Time.deltaTime * acceleration;
            } 
            else if (!_moveBackward  && _velocityZ < 0.0f)
            {
                _velocityZ += Time.deltaTime * decelerationFactor  * acceleration;
            }
            // Left/Right update
            if (_moveLeft && _velocityX > -maxVelocity)
            {
                _velocityX -= Time.deltaTime * acceleration;
            }
            else if (!_moveLeft && _velocityX < 0.0f)
            {
                _velocityX += Time.deltaTime * decelerationFactor  * acceleration;
            }
            if (_moveRight && _velocityX < maxVelocity)
            {
                _velocityX += Time.deltaTime * acceleration;
            }
            else if (!_moveRight && _velocityX > 0.0f)
            {
                _velocityX -= Time.deltaTime * decelerationFactor  * acceleration;
            }
        }

        private void ConstrainVelocity()
        {
            //Forward/Backward constraints
            if (_moveForward && _velocityZ > maxVelocity)
            {
                _velocityZ = maxVelocity;
            }
            if (_moveBackward && _velocityZ < -maxVelocity)
            {
                _velocityZ = -maxVelocity;
            }
            //Left/Right constraints
            if (_moveLeft && _velocityX < -maxVelocity)
            {
                _velocityX = -maxVelocity;
            }
            if (_moveRight && _velocityX > maxVelocity)
            {
                _velocityX = maxVelocity;
            }
        }
        
        void Update()
        {
            _moveForward = Input.GetKey(MoveForwardKey);
            _moveLeft = Input.GetKey(MoveLeftKey);
            _moveRight = Input.GetKey(MoveRightKey);
            _moveBackward = Input.GetKey(MoveBackwardKey);
            
            VelocityUpdate();
            ConstrainVelocity();
            
            _animator.SetFloat(VelocityXHash, _velocityX);
            _animator.SetFloat(VelocityZHash, _velocityZ);
            
            BoundCheck();
        }

        // Places the player on the opposite side of the plane if they try to leave it.
        private void BoundCheck()
        {
            var newPos = new Vector3();
            for (var i = 0; i < 3; i+=2)
            {
                if (transform.position[i] < _planePosition[i]-_planeDimensions[i]/2.0f)
                {
                    newPos[i] = _planePosition[i]+_planeDimensions[i]/2.0f;
                } 
                else if (transform.position[i] > _planePosition[i] + _planeDimensions[i]/2.0f )
                {
                    newPos[i] = _planePosition[i]-_planeDimensions[i]/2.0f;
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
