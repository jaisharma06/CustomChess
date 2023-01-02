using Anvarat.Utils;
using CustomChess.Base;
using CustomChess.Events;
using CustomChess.Pieces.Pawn;
using UnityEngine;

namespace CustomChess.Board
{
    public class ChessBoardCell : MonoBehaviour, IMousePointerEnter, IMousePointerExit, IMousePointerClick
    {
        public PawnController pawn;
        [ReadOnly]
        public Vector2Int Index;

        private Vector3 _workspace;

        private Renderer _renderer;

        private bool IsValid { get; set; }

        public bool IsEmpty { get => pawn == null; }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void SetPos(float x, float y)
        {
            _workspace.Set(x, y, transform.localPosition.z);
            transform.localPosition = _workspace;
        }

        public void Activate()
        {
            IsValid = true;
            _renderer.enabled = true;
        }

        public void Deactivate()
        {
            IsValid = false;
            _renderer.enabled = false;
        }

        public void OnMousePointerExit()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }
            pawn.SetHovered(false);
        }

        public void OnMousePointerEnter()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }

            pawn.SetHovered(true);
        }

        public void OnMousePointerClick()
        {
            if ((DataHandler.instance.CurrentTurn == PlayerType.None) 
                || (!IsValid && 
                (!pawn || 
                (pawn && DataHandler.instance.CurrentTurn != pawn.owner))))
            {
                return;
            }

            if(IsValid) //When the valid cell is empty
            {
                MovePawnToEmptyCell();
                return;
            }

            TogglePawnSelection();
        }

        private void MovePawnToEmptyCell()
        {
            EventManager.PawnStartedMoving?.Invoke();
            DataHandler.instance.SelectedPawn.MoveToTarget(transform.position, () =>
            {
                if(!IsEmpty && DataHandler.instance.SelectedPawn && pawn.owner != DataHandler.instance.SelectedPawn.owner) 
                {
                    pawn.Kill();
                }
                DataHandler.instance.SelectedPawnCell.pawn = null;
                pawn = DataHandler.instance.SelectedPawn;
                DataHandler.instance.SelectedPawn.SetSelected(false);
                EventManager.PawnSelected?.Invoke(this, true);
                DataHandler.instance.SelectedPawn = null;
                DataHandler.instance.SelectedPawnCell = null;
                EventManager.PawnStoppedMoving?.Invoke();
            });
        }

        private void TogglePawnSelection()
        {
            if (DataHandler.instance.SelectedPawn != pawn)
            {
                pawn?.SetSelected(true); //Todo: add cells parameter
                EventManager.PawnSelected?.Invoke(this, false);

                if (DataHandler.instance.SelectedPawn)
                {
                    DataHandler.instance.SelectedPawn.SetSelected(false);
                }

                DataHandler.instance.SelectedPawn = pawn;
                DataHandler.instance.SelectedPawnCell = this;
            }
            else
            {
                pawn?.SetSelected(false);
                EventManager.PawnSelected?.Invoke(this, true);
                DataHandler.instance.SelectedPawn = null;
                DataHandler.instance.SelectedPawnCell = null;
            }
        }
    }
}