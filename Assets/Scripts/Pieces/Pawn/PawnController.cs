using Anvarat.Utils;
using CustomChess.Base;
using CustomChess.Board;
using CustomChess.Pieces.States;
using System;
using System.Collections;
using UnityEngine;

namespace CustomChess.Pieces.Pawn
{
    public class PawnController : Controller
    {
        public PlayerType owner { get; private set; }
        [ReadOnly]
        public ChessBoardCell cell;

        public PieceType pieceType { get => ((PieceData)m_data).pieceType; }
        public bool IsHovered { get; private set; }
        public bool IsSelected { get; private set; }
        public bool IsDead { get; private set; }

        public bool IsFirstMove { get; set; }
        public bool IsMoving { get; private set; }
        public string ID { get; set; }

        private Coroutine CoroutineMoveToTarget;

        private Renderer _renderer;
        private Animator _animator;

        protected override void Awake()
        {
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<Renderer>();
            base.Awake();
        }

        public StatePawnHovered PawnHoveredState { get; private set; }
        public StatePawnUnhovered PawnUnhoveredState { get; private set; }
        public StatePawnSelected PawnSelectedState { get; private set; }
        public StatePawnMove PawnMoveState { get; private set; }

        protected override void SetupStateMachine()
        {
            base.SetupStateMachine();
            PawnHoveredState = new StatePawnHovered(this, _animator, "hovered", _stateMachine, m_data);
            PawnUnhoveredState = new StatePawnUnhovered(this, _animator, "unhovered", _stateMachine, m_data);
            PawnSelectedState = new StatePawnSelected(this, _animator, "selected", _stateMachine, m_data);
            PawnMoveState = new StatePawnMove(this, _animator, "move", _stateMachine, m_data);
            _stateMachine.Initialize(PawnUnhoveredState);
        }

        public void MoveToTarget(Vector3 target, Action onComplete)
        {
            StopMovement();

            CoroutineMoveToTarget = StartCoroutine(IE_MoveToTarget(target, onComplete));
        }

        protected void StopMovement()
        {
            if(CoroutineMoveToTarget != null)
            {
                StopCoroutine(CoroutineMoveToTarget);
            }
        }

        protected IEnumerator IE_MoveToTarget(Vector3 target, Action onComplete)
        {
            IsMoving = true;
            while (Vector3.Distance(transform.position, target) >= 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * ((PieceData)m_data).moveSpeed);
                yield return null;
            }

            transform.position = target;
            IsFirstMove = false;
            IsMoving = false;
            onComplete?.Invoke();
        }

        public void SetMaterial(Material material)
        {
            _renderer.material = material;
        }

        public void SetOwner(PlayerType owner)
        {
            this.owner = owner;
        }

        public void SetPosition(float x, float z)
        {
            var position = transform.position;
            position.x = x;
            position.z = z;
            transform.position = position;
        }

        public void SetHovered(bool hovered)
        {
            IsHovered = hovered;
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
        }

        public void Kill()
        {
            IsDead = true;
            gameObject.SetActive(false);
        }
    }
}