using UnityEngine;

namespace Code.Scripts
{
    [RequireComponent(typeof(Animator))]
    // Controller class for the player to allow movement of the player model in the demo scene. (Mostly taken from Lab1)
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        
        public float acceleration = 1.5f;
        public float decelerationFactor = 1.5f;
        public float maxVelocity = 2.0f;
        public float angularSmoothTime = 0.1f;
        
        private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");
        
        private float _angularSmoothVelocity;
        private Vector3 _velocity;
        private bool _moveForward, _moveBackward, _moveLeft, _moveRight, _freeLook;
        private Animator _animator;

        private const KeyCode MoveForwardKey = KeyCode.W;
        private const KeyCode MoveBackwardKey = KeyCode.S;
        private const KeyCode MoveRightKey = KeyCode.D;
        private const KeyCode MoveLeftKey = KeyCode.A;
        private const KeyCode FreeLookKey = KeyCode.Mouse1;
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            _animator = GetComponent<Animator>();
        }

        private void VelocityUpdate()
        {
            // Forward/Backward update
            if (_moveForward && _velocity.z < maxVelocity)
            {
                _velocity.z += Time.deltaTime * acceleration;
            } 
            else if (!_moveForward && _velocity.z > 0.0f)
            {
                _velocity.z -= Time.deltaTime * decelerationFactor  * acceleration;
            }
            if (_moveBackward && _velocity.z > -maxVelocity)
            {
                _velocity.z -= Time.deltaTime * acceleration;
            } 
            else if (!_moveBackward  && _velocity.z < 0.0f)
            {
                _velocity.z += Time.deltaTime * decelerationFactor  * acceleration;
            }
            // Left/Right update
            if (_moveLeft && _velocity.x > -maxVelocity)
            {
                _velocity.x -= Time.deltaTime * acceleration;
            }
            else if (!_moveLeft && _velocity.x < 0.0f)
            {
                _velocity.x += Time.deltaTime * decelerationFactor  * acceleration;
            }
            if (_moveRight && _velocity.x < maxVelocity)
            {
                _velocity.x += Time.deltaTime * acceleration;
            }
            else if (!_moveRight && _velocity.x > 0.0f)
            {
                _velocity.x -= Time.deltaTime * decelerationFactor  * acceleration;
            }
        }

        private void ConstrainVelocity()
        {
            //Forward/Backward constraints
            if (_moveForward && _velocity.z > maxVelocity)
            {
                _velocity.z = maxVelocity;
            }
            if (_moveBackward && _velocity.z < -maxVelocity)
            {
                _velocity.z = -maxVelocity;
            }
            //Left/Right constraints
            if (_moveLeft && _velocity.x < -maxVelocity)
            {
                _velocity.x = -maxVelocity;
            }
            if (_moveRight && _velocity.x > maxVelocity)
            {
                _velocity.x = maxVelocity;
            }
        }

        private void RotateTowardCamera()
        {
            float targetAngle = camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                targetAngle, ref _angularSmoothVelocity,
                angularSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        void Update()
        {
            _moveForward = Input.GetKey(MoveForwardKey);
            _moveLeft = Input.GetKey(MoveLeftKey);
            _moveRight = Input.GetKey(MoveRightKey);
            _moveBackward = Input.GetKey(MoveBackwardKey);
            _freeLook = Input.GetKey(FreeLookKey);
            
            VelocityUpdate();
            ConstrainVelocity();
            if (!_freeLook && _velocity.magnitude > 0.1f)
            {
                RotateTowardCamera();
            }
            _animator.SetFloat(VelocityXHash, _velocity.x);
            _animator.SetFloat(VelocityZHash, _velocity.z);
        }
    }
}
