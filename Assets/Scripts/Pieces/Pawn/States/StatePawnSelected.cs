using CustomChess.Base;
using CustomChess.Pieces.Pawn;
using UnityEngine;

namespace CustomChess.Pieces.States
{
    public class StatePawnSelected : State
    {
        public StatePawnSelected(Controller controller, Animator anim, string animationName, StateMachine stateMachine, Data data) : base(controller, anim, animationName, stateMachine, data)
        {
        }

        public override void Tick()
        {
            base.Tick();

            if (!((PawnController)controller).IsSelected)
            {
                stateMachine.SwitchState(((PawnController)controller).PawnUnhoveredState);
            }else if (((PawnController)controller).IsMoving)
            {
                stateMachine.SwitchState(((PawnController)controller).PawnMoveState);
            }
        }
    }
}
