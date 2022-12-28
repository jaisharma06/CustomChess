namespace CustomChess.Base
{
    public class StateMachine
    {
        private State _currentState;

        public void Initialize(State currentState)
        {
            _currentState = currentState;
            _currentState?.Enter();
        }

        public void SwitchState(State nextState)
        {
            if(nextState == null || _currentState == nextState)
            {
                return;
            }

            _currentState?.Exit();
            _currentState = nextState;
            _currentState.Enter();
        }

        public void Tick()
        {
            _currentState?.Tick();
        }

        public void PhysicsTick()
        {
            _currentState.PhysicsTick();
        }

        public void AnimationStartTrigger() => _currentState.AnimationStartTrigger();
        public void AnimationEndTrigger() => _currentState.AnimationEndTrigger();
        public void AnimationTrigger() => _currentState.AnimationTrigger();
    }
}