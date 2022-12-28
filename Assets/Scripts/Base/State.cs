using UnityEngine;

namespace CustomChess.Base
{
    public class State
    {
        protected Controller controller;
        protected Animator anim;
        protected string animationName;
        protected StateMachine stateMachine;
        protected Data data;

        public State(Controller controller, Animator anim, string animationName, StateMachine stateMachine, Data data) 
        {
            this.controller = controller;
            this.anim = anim;
            this.animationName = animationName;
            this.stateMachine = stateMachine;
            this.data = data;
        }

        public virtual void Enter() 
        {
            DoCheck();
        }

        public virtual void Exit() { }

        public virtual void Tick() { }

        public virtual void PhysicsTick()
        {
            DoCheck();
        }

        public virtual void DoCheck() { }

        public virtual void AnimationStartTrigger() { }
        public virtual void AnimationEndTrigger() { }
        public virtual void AnimationTrigger() { }
    }
}