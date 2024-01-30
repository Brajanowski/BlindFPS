using Pause;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterLocomotion _characterLocomotion;

        [SerializeField]
        private FirstPersonCamera _firstPersonCamera;

        [SerializeField]
        private Gun _gun;

        [SerializeField]
        private float _environmentScanCooldown = 0.5f;

        [SerializeField]
        private float _gunKnockback = 15.0f;

        private GameControls _gameControls;

        private float _nextScanTime;

        public UnityEvent OnEnvironmentScan = new();

        private void Awake()
        {
            _gameControls = new GameControls();
        }

        private void OnEnable()
        {
            _gameControls.Enable();
            _gameControls.FPP.Scan.performed += OnScan;
            _gameControls.FPP.Jump.performed += OnJump;
            _gameControls.FPP.Sprint.started += OnSprintStart;
            _gameControls.FPP.Sprint.canceled += OnSprintStop;
            _gameControls.FPP.Fire.performed += OnFire;
            _gameControls.FPP.Pause.performed += OnPause;
            
            PauseMenu.Instance.OnShow.AddListener(OnPauseMenuShow);
            PauseMenu.Instance.OnHide.AddListener(OnPauseMenuHide);
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            _gameControls.FPP.Sprint.started += OnSprintStart;
            _gameControls.FPP.Sprint.canceled += OnSprintStop;
            _gameControls.FPP.Jump.performed -= OnJump;
            _gameControls.FPP.Scan.performed -= OnScan;
            _gameControls.FPP.Fire.performed -= OnFire;
            _gameControls.FPP.Pause.performed -= OnPause;
            
            PauseMenu.Instance.OnShow.RemoveListener(OnPauseMenuShow);
            PauseMenu.Instance.OnHide.RemoveListener(OnPauseMenuHide);
            
            _gameControls.Disable();
            Cursor.lockState = CursorLockMode.None;
        }

        public FirstPersonCamera GetCameraController() => _firstPersonCamera;

        private void OnPause(InputAction.CallbackContext ctx)
        {
            PauseMenu.Instance.Toggle();
        }

        private void MoveRight(float input)
        {
            Vector3 direction = Quaternion.AngleAxis(_firstPersonCamera.Yaw, Vector3.up) * Vector3.right;
            _characterLocomotion.AddMovementInput(direction, input);
        }

        private void MoveForward(float input)
        {
            Vector3 direction = Quaternion.AngleAxis(_firstPersonCamera.Yaw, Vector3.up) * Vector3.forward;
            _characterLocomotion.AddMovementInput(direction, input);
        }

        private void Update()
        {
            Vector2 move = _gameControls.FPP.Move.ReadValue<Vector2>();
            MoveRight(move.x);
            MoveForward(move.y);

            Vector2 look = _gameControls.FPP.Look.ReadValue<Vector2>();
            _firstPersonCamera.Rotate(look.x, look.y);
        }
        
        private void OnScan(InputAction.CallbackContext ctx)
        {
            if (Time.time < _nextScanTime)
            {
                return;
            }

            OnEnvironmentScan?.Invoke();
            _nextScanTime = Time.time + _environmentScanCooldown;
        }

        private void OnSprintStart(InputAction.CallbackContext ctx)
        {
            _characterLocomotion.StartSprint();
        }

        private void OnSprintStop(InputAction.CallbackContext ctx)
        {
            _characterLocomotion.StopSprint();
        }
        
        private void OnJump(InputAction.CallbackContext ctx)
        {
            _characterLocomotion.Jump();
        }
        
        private void OnFire(InputAction.CallbackContext ctx)
        {
            Vector3 shootDir = _firstPersonCamera.Camera.transform.forward;
            if (_gun.Shoot(_firstPersonCamera.transform.position, shootDir))
            {
                _characterLocomotion.AddVelocity(shootDir * -_gunKnockback);
            }
        }
        
        private void OnPauseMenuShow()
        {
            _gameControls.Disable();
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnPauseMenuHide()
        {
            _gameControls.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}