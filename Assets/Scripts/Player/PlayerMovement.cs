using System;
using Drinkables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    [RequireComponent(typeof(Animator))]
    // Controller class for the player to allow movement of the player model in the demo scene. (Mostly taken from Lab1)
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform thirdPersonCamera;
        [SerializeField] private float acceleration = 1.5f;
        [SerializeField] private float decelerationFactor = 1.5f;
        [SerializeField] private Vector3 maxVelocities = new Vector3(2.0f,0.0f,2.0f);
        [SerializeField] private float angularSmoothTime = 0.1f;

        private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");

        private float _angularSmoothVelocity;
        private Vector3 _velocity;
        private Vector3 _velocityJiggle;
        private Vector3 _currentMaxVelocities;
        private Vector3 _currentMinVelocities;
        
        private bool _moveForward, _moveBackward, _moveLeft, _moveRight, _freeLook;
        
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            
            _currentMaxVelocities= maxVelocities;
            _currentMinVelocities = new Vector3(0.0f,0.0f,0.0f);

            InvokeRepeating(nameof(VelocityJiggle), 0.0f, 1.0f);
            InvokeRepeating(nameof(JiggleReset), 0.5f, 1.0f);
        }

        private void VelocityUpdate()
        {
            // Forward/Backward update
            if (_moveForward && _velocity.z < _currentMaxVelocities.z)
            {
                _velocity.z += Time.deltaTime * acceleration;
            }
            else if (!_moveForward && _velocity.z > 0.0f)
            {
                _velocity.z -= Time.deltaTime * decelerationFactor * acceleration;
            }

            if (_moveBackward && _velocity.z > -_currentMaxVelocities.z)
            {
                _velocity.z -= Time.deltaTime * 2.0f * acceleration;
            }
            else if (!_moveBackward && _velocity.z < 0.0f)
            {
                _velocity.z += Time.deltaTime * decelerationFactor * acceleration;
            }

            // Left/Right update
            if (_moveLeft && _velocity.x > -_currentMaxVelocities.x)
            {
                _velocity.x -= Time.deltaTime * acceleration;
            }
            else if (!_moveLeft && _velocity.x < 0.0f)
            {
                _velocity.x += Time.deltaTime * decelerationFactor * acceleration;
            }

            if (_moveRight && _velocity.x < _currentMaxVelocities.x)
            {
                _velocity.x += Time.deltaTime * acceleration;
            }
            else if (!_moveRight && _velocity.x > 0.0f)
            {
                _velocity.x -= Time.deltaTime * decelerationFactor * acceleration;
            }
        }

        private void ConstrainVelocity()
        {
            //Forward/Backward constraints
            if (_moveForward && _velocity.z > _currentMaxVelocities.z)
            {
                if (_currentMaxVelocities.z < maxVelocities.z)
                {
                    _velocity.z -= Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.z = maxVelocities.z;
                }
            }

            if (_moveBackward && _velocity.z < -_currentMaxVelocities.z)
            {
                if (-_currentMaxVelocities.z > -maxVelocities.z)
                {
                    _velocity.z += Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.z = -maxVelocities.z;
                }
            }

            //Left/Right constraints
            if (_moveLeft && _velocity.x < -_currentMaxVelocities.x)
            {
                if (-_currentMaxVelocities.x > -maxVelocities.x)
                {
                    _velocity.x += Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.x = -maxVelocities.x;
                }
            }

            if (_moveRight && _velocity.x > _currentMaxVelocities.x)
            {
                if (_currentMaxVelocities.x < maxVelocities.x)
                {
                    _velocity.x -= Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.x = maxVelocities.x;
                }
            }
            
            // Min velocity checks
            if (_currentMinVelocities.z != 0.0f)
            {
                if (_moveLeft || _moveRight)
                {
                    if (!_moveBackward && _currentMinVelocities.z > 0.0f && _velocity.z < _currentMinVelocities.z)
                    {
                        _velocity.z += Time.deltaTime * 2.0f *  acceleration;
                    }
                    if (!_moveForward && _currentMinVelocities.z < 0.0f && _velocity.z > _currentMinVelocities.z)
                    {
                        _velocity.z -= Time.deltaTime * 2.0f *  acceleration;
                    }  
                }
            } 
            else if (!_moveForward && !_moveBackward && _velocity.z < 0.1f && _velocity.z > -0.1f)
            {
                _velocity.z = 0.0f;
            }
            
            if (_currentMinVelocities.x!= 0.0f)
            {
                if (_moveForward || _moveBackward)
                {
                    if (!_moveRight && _currentMinVelocities.x < 0.0f && _velocity.x > _currentMinVelocities.x)
                    {
                        _velocity.x -= Time.deltaTime * 2.0f * acceleration;
                    }

                    if (!_moveLeft && _currentMinVelocities.x > 0.0f && _velocity.x < _currentMinVelocities.x)
                    {
                        _velocity.x += Time.deltaTime * 2.0f * acceleration;
                    }
                }
            }
            else if (!_moveLeft && !_moveRight && _velocity.x < 0.1f && _velocity.x > -0.1f)
            {
                _velocity.x = 0.0f;
            }
        }

        private void RotateTowardCamera()
        {
            float targetAngle = thirdPersonCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                targetAngle, ref _angularSmoothVelocity,
                angularSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        void Update()
        {
            _moveForward = Input.GetKey(Indestructibles.MovementControls.MoveForwardKey);
            _moveLeft = Input.GetKey(Indestructibles.MovementControls.MoveLeftKey);
            _moveRight = Input.GetKey(Indestructibles.MovementControls.MoveRightKey);
            _moveBackward = Input.GetKey(Indestructibles.MovementControls.MoveBackwardKey);

            VelocityUpdate();
            ConstrainVelocity();

            if (_velocity.magnitude > 0.1f)
            {
                RotateTowardCamera();
            }

            Indestructibles.PlayerAnimator.SetFloat(VelocityXHash, _velocity.x);
            Indestructibles.PlayerAnimator.SetFloat(VelocityZHash, _velocity.z);
            
        }

        void VelocityJiggle()
        {
            var intoxication = Indestructibles.PlayerData.IntoxicationLevel;
            
            _currentMaxVelocities.x = maxVelocities.x - Random.Range(0.0f, intoxication/2.0f);
            _currentMaxVelocities.z = maxVelocities.z - Random.Range(0.0f, intoxication/2.0f);
            
            _currentMinVelocities.x = Random.Range(-intoxication/2.0f, intoxication/2.0f);
            _currentMinVelocities.z = Random.Range(-intoxication/2.0f, intoxication/2.0f);
        }

        void JiggleReset()
        {
            _currentMaxVelocities = maxVelocities;
            
            _currentMinVelocities.x = 0.0f;
            _currentMinVelocities.z = 0.0f;
        }
        
    }
}