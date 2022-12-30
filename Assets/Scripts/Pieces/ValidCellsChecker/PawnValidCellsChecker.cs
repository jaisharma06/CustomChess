using CustomChess.Board;
using System.Collections.Generic;
using UnityEngine;

namespace CustomChess.Pieces
{
    public class PawnValidCellsChecker : MonoBehaviour
    {
        public List<ChessBoardCell> GetValidCells(PieceType pieceType, Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    return GetMovableCellsForPawn(pawnCell, cells);
                default:return new List<ChessBoardCell>();
            } 
        }

        private List<ChessBoardCell> GetMovableCellsForPawn(Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            if (cells == null)
            {
                return null;
            }

            List<ChessBoardCell> movableCells = new List<ChessBoardCell>();
            
            //Forward cell
            if(pawnCell.x < ChessBoard.BOARD_COLS - 1)
            {
                if(cells[pawnCell.x + 1, pawnCell.y].IsEmpty)
                {
                    movableCells.Add(cells[pawnCell.x + 1, pawnCell.y]);  
                }
            }

            //Left diagonal cell
            if (pawnCell.x < ChessBoard.BOARD_COLS - 1 && pawnCell.y > 0)
            {
                if (!cells[pawnCell.x + 1, pawnCell.y - 1].IsEmpty)
                {
                    movableCells.Add(cells[pawnCell.x + 1, pawnCell.y - 1]);
                }
            }

            //Right diagonal cell
            if (pawnCell.x < ChessBoard.BOARD_COLS - 1 && pawnCell.y < ChessBoard.BOARD_ROWS - 1)
            {
                if (!cells[pawnCell.x + 1, pawnCell.y + 1].IsEmpty)
                {
                    movableCells.Add(cells[pawnCell.x + 1, pawnCell.y + 1]);
                }
            }

            return movableCells;
        }
    }
}