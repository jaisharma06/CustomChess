using CustomChess.Base;
using System;
using System.Collections;
using UnityEngine;

namespace CustomChess.Pieces.Pawn
{
    public class PawnController : Controller
    {
        private Coroutine CoroutineMoveToTarget;
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
    }
}