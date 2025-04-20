using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class IdleState : BasePlayerState
    {
        #region constructors
        
        public IdleState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            playerMachine.PlayerModel.isGrounded = true;
            playerMachine.PlayerModel.coyoteTime = dataSo.jumpData.jumpCoyoteTime;
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            HandleInputGather();
            
            OnFall();
            OnJump();
            OnMove();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            HandleRotateInputGather();
            HandleGravity();
            Move(playerMachine.PlayerModel.currentMoveMultiplier);
            Look();
            return 0;
        }

        private void OnMove()
        {
            if (inputDir.sqrMagnitude > 0)
                stateMachine.SwitchState("move");
        }

        private void OnJump()
        {
            if (playerMachine.PlayerModel.jumpBufferTime > 0)
                stateMachine.SwitchState("jump");
        }

        private void OnFall()
        {
            if (CheckGround())
                return;
            
            stateMachine.SwitchState("fall");
        }

        #endregion
    }
}