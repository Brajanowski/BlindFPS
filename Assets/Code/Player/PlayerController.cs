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
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            _gameControls.FPP.Sprint.started += OnSprintStart;
            _gameControls.FPP.Sprint.canceled += OnSprintStop;
            _gameControls.FPP.Jump.performed -= OnJump;
            _gameControls.FPP.Scan.performed -= OnScan;
            _gameControls.FPP.Fire.performed -= OnFire;
            
            _gameControls.Disable();
            Cursor.lockState = CursorLockMode.None;
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
#if UNITY_EDITOR
            if (Cursor.lockState == CursorLockMode.Locked && Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                return;
            }

            if (Cursor.lockState == CursorLockMode.None)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                return;
            }
#endif

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
            _gun.Shoot(_firstPersonCamera.transform.position, _firstPersonCamera.Camera.transform.forward);
        }
    }
}