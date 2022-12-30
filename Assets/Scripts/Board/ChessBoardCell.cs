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
            if (!pawn || DataHandler.instance.CurrentTurn != pawn.owner)
            {
                return;
            }

            if(DataHandler.instance.SelectedPawn != pawn)
            {
                pawn.SetSelected(true); //Todo: add cells parameter
                EventManager.PawnSelected?.Invoke(this, false);

                if (DataHandler.instance.SelectedPawn)
                {
                    DataHandler.instance.SelectedPawn.SetSelected(false);
                }
                
                DataHandler.instance.SelectedPawn = pawn;
            }
            else
            {
                pawn.SetSelected(false);
                EventManager.PawnSelected?.Invoke(this, true);
                DataHandler.instance.SelectedPawn = null;
            }
        }
    }
}