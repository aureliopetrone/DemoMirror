using Mirror;
using Mirror.Examples.TankTheftAuto;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DemoMirror
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private TankController controller = null;

        [ClientCallback]
        private void OnEnable() => InputManager.Controls.Enable();

        [ClientCallback]
        private void OnDisable() => InputManager.Controls.Disable();

        private Vector2 previousInput;

        public override void OnStartAuthority()
        {
            enabled = true;

            InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            InputManager.Controls.Player.Move.canceled += ctx => ResetMovement();
        }

        [ClientCallback]
        private void Update() => Move();

        [Client]
        private void SetMovement(Vector2 movement) => previousInput = movement;

        [Client]
        private void ResetMovement() => previousInput = Vector2.zero;

        [Client]
        private void Move()
        {
            Debug.Log("Previous Input: " + previousInput);
            controller.MoveForward(previousInput.y);

            if(previousInput.x != 0)
            {

                controller.TurnRight(previousInput.x);
                // Should I turn right?
              
            }
        }
    }
}
