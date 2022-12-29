using CustomChess.Base;
using CustomChess.Pieces.Pawn;
using UnityEngine;

namespace CustomChess.Pieces.States
{
    public class StatePawnUnhovered : State
    {
        public StatePawnUnhovered(Controller controller, Animator anim, string animationName, StateMachine stateMachine, Data data) : base(controller, anim, animationName, stateMachine, data)
        {
        }

        public override void Tick()
        {
            base.Tick();

            if (((PawnController)controller).IsHovered)
            {
                stateMachine.SwitchState(((PawnController)controller).PawnHoveredState);
            }
        }
    }
}