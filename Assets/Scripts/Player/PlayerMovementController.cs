using System.Collections.Generic;
using Drinkables;
using Player;
using UnityEngine;

namespace Code.Scripts
{
    [RequireComponent(typeof(Animator))]
    // Controller class for the player to allow movement of the player model in the demo scene. (Mostly taken from Lab1)
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private float acceleration = 1.5f;
        [SerializeField] private float decelerationFactor = 1.5f;
        [SerializeField] private float maxVelocity = 2.0f;
        [SerializeField] private float angularSmoothTime = 0.1f;

        private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");

        private float _angularSmoothVelocity;
        private Vector3 _velocity;

        private float _drunkennessTimer = 1.0f;
        private List<AbstractDrinkable> _drinksStillInBody = new List<AbstractDrinkable>();
        
        private bool _moveForward, _moveBackward, _moveLeft, _moveRight, _freeLook;
        
        private Animator _animator;
        
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

            _animator.SetFloat(VelocityXHash, _velocity.x);
            _animator.SetFloat(VelocityZHash, _velocity.z);
            
            // the next few lines are just for debugging purposes
            // when you press U, the player will drink a beer and the beer's effects will take place
            // it does not currently handle the case where water is drank --> this is just to give
            // an idea in regards to the general architecture.
            _drunkennessTimer -= Time.deltaTime;

            // check every second to see if one of the beers has left the body
            // beers currently last 6 seconds in the body for testing purposes
            if (_drunkennessTimer <= 0.0f)
            {
                _drunkennessTimer = 1.0f;
                DrunkennessHandler.UpdateBeerBelly(_drinksStillInBody, _drunkennessTimer);
            }

            // drink a regular beer every time the player presses U
            if (Input.GetKeyDown(KeyCode.U))
            {
                _drinksStillInBody.Add(new UpsideDownBeer());
                DrunkennessHandler.OnDrink(_drinksStillInBody[_drinksStillInBody.Count - 1]);
            }
        }
    }
}
