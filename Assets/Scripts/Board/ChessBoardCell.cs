using CustomChess.Base;
using CustomChess.Pieces.Pawn;
using UnityEngine;

namespace CustomChess.Board
{
    public class ChessBoardCell : MonoBehaviour, IMousePointerEnter, IMousePointerExit, IMousePointerClick
    {
        public PawnController pawn;

        private Vector3 _workspace;

        private Renderer _renderer;

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
            _renderer.enabled = true;
        }

        public void Deactivate()
        {
            _renderer.enabled = false;
        }

        public void OnMousePointerExit()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }
            pawn.SetUnHovered();
        }

        public void OnMousePointerEnter()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }

            pawn.SetHovered();
        }

        public void OnMousePointerClick()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }

            if(DataHandler.instance.SelectedPawn != pawn)
            {
                DataHandler.instance.SelectedPawn = pawn;
            }
            else
            {
                DataHandler.instance.SelectedPawn = null;
            }
        }
    }
}