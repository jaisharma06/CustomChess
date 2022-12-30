using CustomChess.Base;
using CustomChess.Board;
using CustomChess.Pieces.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomChess.Pieces.Pawn
{
    public class PawnController : Controller
    {
        public PlayerType owner { get; private set; }

        public PieceType pieceType { get => ((PieceData)m_data).pieceType; }
        public bool IsHovered { get; private set; }
        public bool IsSelected { get; private set; }

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

        protected override void SetupStateMachine()
        {
            base.SetupStateMachine();
            PawnHoveredState = new StatePawnHovered(this, _animator, "hovered", _stateMachine, m_data);
            PawnUnhoveredState = new StatePawnUnhovered(this, _animator, "unhovered", _stateMachine, m_data);
            PawnSelectedState = new StatePawnSelected(this, _animator, "selected", _stateMachine, m_data);
            _stateMachine.Initialize(PawnUnhoveredState);
        }

        protected void MoveToTarget(Vector3 target, Action onComplete)
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
            while (Vector3.Distance(transform.position, target) <= 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * ((PieceData)m_data).moveSpeed);
                yield return null;
            }

            transform.position = target;
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
    }
}