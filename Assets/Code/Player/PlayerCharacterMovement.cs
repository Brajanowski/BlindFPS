using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Player
{
    public class PlayerCharacterMovement : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _characterController;

        [SerializeField]
        private float _maxMovementSpeed = 10f;

        [SerializeField]
        private float _maxSprintSpeed = 15.0f;

        [SerializeField]
        private float _jumpHeight = 1.0f;

        [SerializeField]
        private float _gravity = 10f;

        [SerializeField]
        private float _maxFallSpeed = 50f;

        [SerializeField]
        private float _damping = 40.0f;

        [SerializeField]
        private FootstepEmitter[] _footstepEmitters;

        [SerializeField]
        private Vector2 _foostepEmitSlowFastRate = new Vector2(1.0f, 0.2f);

        [SerializeField]
        private Vector2 _footstepEmitSpeedSlowFastThresholds = new Vector2(3.0f, 10.0f);

        private float _emitFootstepTimer;
        private int _footstepEmitterIndex;

        private Vector3 _pendingInputValue;
        private Vector3 _currentVelocity;

        private bool _isSprinting;
        private bool _jumpRequested;
        private bool _isFalling;

        public bool IsGrounded => _characterController.isGrounded;

        public Vector3 Velocity => _currentVelocity;

        public UnityEvent OnJumpStarted = new();
        public UnityEvent OnLanded = new();

        private void Update()
        {
            ConsumePendingInput();
            UpdateVerticalVelocity();

            _characterController.Move(_currentVelocity * Time.deltaTime);

            HandleFootsteps();

            ApplyFriction();
            DetectFall();
        }

        private void HandleFootsteps()
        {
            if (!IsGrounded)
            {
                _emitFootstepTimer = 1.0f;
                return;
            }
            
            Vector3 vel = _currentVelocity;
            vel.y = 0.0f;

            float speed = vel.magnitude;
            
            float t = Mathf.Clamp01(MathfUtils.Remap(speed, _footstepEmitSpeedSlowFastThresholds.x, _footstepEmitSpeedSlowFastThresholds.y, 0.0f, 1.0f));
            float rate = Mathf.Lerp(_foostepEmitSlowFastRate.x, _foostepEmitSlowFastRate.y, t);

            _emitFootstepTimer -= Time.deltaTime * rate;
            if (_emitFootstepTimer > 0.0f)
            {
                return;
            }

            _emitFootstepTimer = 1.0f;

            if (speed < _footstepEmitSpeedSlowFastThresholds.x)
            {
                return;
            }

            _footstepEmitterIndex++;
            if (_footstepEmitterIndex > _footstepEmitters.Length - 1)
            {
                _footstepEmitterIndex = 0;
            }

            _footstepEmitters[_footstepEmitterIndex].Emit();
        }

        private void DetectFall()
        {
            if (_isFalling && _characterController.isGrounded)
            {
                if (Velocity.y < -2.0f)
                {
                    OnLanded?.Invoke();
                }

                _isFalling = false;
            }
            else if (!_characterController.isGrounded)
            {
                _isFalling = true;
            }
        }

        private void ConsumePendingInput()
        {
            if (!IsGrounded)
            {
                _pendingInputValue = Vector3.zero;
                return;
            }

            _pendingInputValue.y = 0.0f;

            float pendingInputMagnitude = _pendingInputValue.magnitude;
            Vector3 direction = Vector3.zero;

            if (pendingInputMagnitude > 0.0f)
            {
                // normalize vector, reusing magnitude to avoid multiple sqrt calls
                direction = _pendingInputValue / pendingInputMagnitude;
            }

            float movementSpeed = _isSprinting ? _maxSprintSpeed : _maxMovementSpeed;
            Vector3 movementVelocity = _currentVelocity;
            movementVelocity.y = 0.0f;
            movementVelocity += direction * (movementSpeed * pendingInputMagnitude);
            movementVelocity.y = 0.0f;

            // Clamp speed
            float movementVelocityMagnitude = movementVelocity.magnitude;
            Vector3 movementVelocityDirection = movementVelocityMagnitude > 0.0f ? movementVelocity / movementVelocityMagnitude : Vector3.zero;

            if (movementVelocityMagnitude > movementSpeed)
            {
                movementVelocityMagnitude = movementSpeed;
            }

            movementVelocity = movementVelocityDirection * movementVelocityMagnitude;

            _currentVelocity.x = movementVelocity.x;
            _currentVelocity.z = movementVelocity.z;

            _pendingInputValue = Vector3.zero;
        }

        private void UpdateVerticalVelocity()
        {
            if (_characterController.isGrounded)
            {
                if (_jumpRequested)
                {
                    _currentVelocity.y = Mathf.Sqrt(2.0f * _gravity * _jumpHeight);
                    _jumpRequested = false;

                    OnJumpStarted?.Invoke();
                }
                else
                {
                    _currentVelocity.y = -1f;
                }
            }
            else
            {
                _currentVelocity.y -= _gravity * Time.deltaTime;
                _currentVelocity.y = Mathf.Max(_currentVelocity.y, -_maxFallSpeed);
            }
        }

        private void ApplyFriction()
        {
            if (!IsGrounded)
            {
                return;
            }

            Vector3 movementVelocity = _currentVelocity;
            movementVelocity.y = 0.0f;

            movementVelocity = Vector3.MoveTowards(movementVelocity, Vector3.zero, _damping * Time.deltaTime);

            _currentVelocity.x = movementVelocity.x;
            _currentVelocity.z = movementVelocity.z;
        }

        public void AddVelocity(Vector3 velocity)
        {
            _currentVelocity += velocity;
        }

        public void AddMovementInput(Vector3 input, float scale)
        {
            _pendingInputValue += input * scale;
        }

        public void Jump()
        {
            if (!_characterController.isGrounded)
            {
                return;
            }

            _jumpRequested = true;
        }

        public void StartSprint()
        {
            _isSprinting = true;
        }

        public void StopSprint()
        {
            _isSprinting = false;
        }
    }
}