using CustomChess.Base;
using CustomChess.Pieces.Pawn;
using UnityEngine;

namespace CustomChess.Pieces.States
{
    public class StatePawnHovered : State
    {
        public StatePawnHovered(Controller controller, Animator anim, string animationName, StateMachine stateMachine, Data data) : base(controller, anim, animationName, stateMachine, data)
        {
        }

        public override void Tick()
        {
            base.Tick();
            if (((PawnController)controller).IsSelected)
            {
                stateMachine.SwitchState(((PawnController)controller).PawnSelectedState);
            }
            else if (!((PawnController)controller).IsHovered)
            {
                stateMachine.SwitchState(((PawnController)controller).PawnUnhoveredState);
            }
        }
    }
}