using UnityEngine;

namespace CustomChess.Base
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] protected Data m_data;

        protected StateMachine _stateMachine;

        protected virtual void Awake()
        {
            SetupStateMachine();
        }

        protected virtual void SetupStateMachine()
        {
            _stateMachine = new StateMachine();
        }

        #region Animation
        public void AnimationStartTrigger() => _stateMachine.AnimationStartTrigger();
        public void AnimationEndTrigger() => _stateMachine.AnimationEndTrigger();
        public void AnimationTrigger() => _stateMachine.AnimationTrigger();
        #endregion
    }
}
