using CustomChess.Base;
using CustomChess.Pieces.Pawn;
using UnityEngine;

namespace CustomChess.Board
{
    public class ChessBoardCell : MonoBehaviour, IMouseEnter, IMouseExit, IMouseClick, IMouseDown, IMouseUp
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

        public void OnMouseExit()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }
            pawn.SetUnHovered();
        }

        public void OnMouseEnter()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }

            pawn.SetHovered();
        }

        public void OnMouseClick()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }
            Debug.Log($"Mouse Clicked on: {pawn.name}");
        }

        public void OnMouseDown()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }

            Debug.Log($"Mouse Down on: {pawn.name}");
        }

        public void OnMouseUp()
        {
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }
            Debug.Log($"Mouse Up on: {pawn.name}");
        }
    }
}