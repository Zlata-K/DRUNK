using System;
using Drinkables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    [RequireComponent(typeof(Animator))]
    // Controller class for the player to allow movement of the player model in the demo scene. (Mostly taken from Lab1)
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Transform thirdPersonCamera;
        [SerializeField] private float acceleration = 1.5f;
        [SerializeField] private float decelerationFactor = 1.5f;
        [SerializeField] private float maxVelocity = 2.0f;
        [SerializeField] private float angularSmoothTime = 0.1f;

        private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");

        private float _angularSmoothVelocity;
        private Vector3 _velocity;
        private Vector3 _velocityJiggle;
        private float _currentMaxVelocity;
        private float _currentMinVelocity;
        private bool _moveForward, _moveBackward, _moveLeft, _moveRight, _freeLook;

        private GameObject _belly;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            _currentMaxVelocity = maxVelocity;
            _belly = GameObject.Find("Belly");
            InvokeRepeating($"VelocityJiggle", 0.0f, 2.0f);
            InvokeRepeating($"JiggleReset", 1.0f, 2.0f);
        }

        private void VelocityUpdate()
        {
            // Forward/Backward update
            if (_moveForward && _velocity.z < _currentMaxVelocity)
            {
                _velocity.z += Time.deltaTime * acceleration;
            }
            else if (!_moveForward && _velocity.z > 0.0f)
            {
                _velocity.z -= Time.deltaTime * decelerationFactor * acceleration;
            }

            if (_moveBackward && _velocity.z > -_currentMaxVelocity)
            {
                _velocity.z -= Time.deltaTime * 2.0f * acceleration;
            }
            else if (!_moveBackward && _velocity.z < 0.0f)
            {
                _velocity.z += Time.deltaTime * decelerationFactor * acceleration;
            }

            // Left/Right update
            if (_moveLeft && _velocity.x > -_currentMaxVelocity)
            {
                _velocity.x -= Time.deltaTime * acceleration;
            }
            else if (!_moveLeft && _velocity.x < 0.0f)
            {
                _velocity.x += Time.deltaTime * decelerationFactor * acceleration;
            }

            if (_moveRight && _velocity.x < _currentMaxVelocity)
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
            if (_moveForward && _velocity.z > _currentMaxVelocity)
            {
                if (_currentMaxVelocity < maxVelocity)
                {
                    _velocity.z -= Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.z = maxVelocity;
                }
            }

            if (_moveBackward && _velocity.z < -_currentMaxVelocity)
            {
                if (-_currentMaxVelocity > -maxVelocity)
                {
                    _velocity.z += Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.z = -maxVelocity;
                }
            }

            //Left/Right constraints
            if (_moveLeft && _velocity.x < -_currentMaxVelocity)
            {
                if (-_currentMaxVelocity > -maxVelocity)
                {
                    _velocity.x += Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.x = -maxVelocity;
                }
            }

            if (_moveRight && _velocity.x > _currentMaxVelocity)
            {
                if (_currentMaxVelocity < maxVelocity)
                {
                    _velocity.x -= Time.deltaTime * 2.0f *  acceleration;
                }
                else
                {
                    _velocity.x = maxVelocity;
                }
            }
            
            // Min velocity checks
            if (_currentMinVelocity != 0.0f)
            {
                if (_moveLeft || _moveRight)
                {
                    if (!_moveForward && _velocity.z > 0.0f && _velocity.z < _currentMinVelocity)
                    {
                        _velocity.z += Time.deltaTime * 2.0f *  acceleration;
                    }
                    if (!_moveBackward && _velocity.z < 0.0f && _velocity.z > _currentMinVelocity)
                    {
                        _velocity.z -= Time.deltaTime * 2.0f *  acceleration;
                    }  
                }

                if (_moveForward || _moveBackward)
                {
                    if (!_moveLeft && _velocity.x < 0.0f && _velocity.x > _currentMinVelocity)
                    {
                        _velocity.x -= Time.deltaTime * 2.0f *  acceleration;
                    }
                    if (!_moveRight && _velocity.z > 0.0f &&  _velocity.x < _currentMinVelocity)
                    {
                        _velocity.x += Time.deltaTime * 2.0f *  acceleration;
                    }
                }
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
            _moveForward = Input.GetKey(Indestructibles.Controls.MoveForwardKey);
            _moveLeft = Input.GetKey(Indestructibles.Controls.MoveLeftKey);
            _moveRight = Input.GetKey(Indestructibles.Controls.MoveRightKey);
            _moveBackward = Input.GetKey(Indestructibles.Controls.MoveBackwardKey);

            VelocityUpdate();
            ConstrainVelocity();

            if (_velocity.magnitude > 0.1f)
            {
                RotateTowardCamera();
            }

            Indestructibles.PlayerAnimator.SetFloat(VelocityXHash, _velocity.x);
            Indestructibles.PlayerAnimator.SetFloat(VelocityZHash, _velocity.z);

            // the next few lines are just for debugging purposes
            // when you press U, the player will drink a beer and the beer's effects will take place
            // it does not currently handle the case where water is drank --> this is just to give
            // an idea in regards to the general architecture.

            // drink a regular beer every time the player presses U
            if (Input.GetKeyDown(KeyCode.U))
            {
                var beer = new GameObject();
                beer.transform.parent = transform;
                beer.AddComponent<RegularBeer>();
            }
        }

        void VelocityJiggle()
        {
            var intoxication = Indestructibles.PlayerData.IntoxicationLevel;
            _currentMaxVelocity = maxVelocity - Random.Range(0.0f, intoxication/2.0f);
            _currentMinVelocity = Random.Range(-intoxication/2.0f, intoxication/2.0f);
        }

        void JiggleReset()
        {
            _currentMaxVelocity = maxVelocity;
            _currentMinVelocity = 0.0f;
        }
    }
}