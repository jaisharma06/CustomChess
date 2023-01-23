using CustomChess.Board;
using CustomChess.Pieces.Pawn;
using System.Collections.Generic;
using UnityEngine;

namespace CustomChess.Pieces
{
    public class LegalMoveMap
    {
        public PawnController pawn;
        public List<ChessBoardCell> legalMoves;

        public LegalMoveMap(PawnController pawn, List<ChessBoardCell> legalMoves)
        {
            this.pawn = pawn;
            this.legalMoves = legalMoves;
        }

        public LegalMoveMap()
        {
            pawn = null;
            legalMoves = null;
        }
    }

    public class PawnValidCellsChecker : MonoBehaviour
    {
        public Dictionary<string, LegalMoveMap> whitePawnsLegalMovesMap = new Dictionary<string, LegalMoveMap>();
        public Dictionary<string, LegalMoveMap> blackPawnsLegalMovesMap = new Dictionary<string, LegalMoveMap>();

        public void InitializeLegalMovesMap(List<PawnController> pawns, PlayerType owner)
        {
            switch (owner)
            {
                case PlayerType.Player1:
                    pawns.ForEach(p =>
                    {
                        whitePawnsLegalMovesMap.Add(p.ID, new LegalMoveMap(p, null));
                    });
                    break;
                case PlayerType.Player2:
                case PlayerType.AI:
                    pawns.ForEach(p =>
                    {
                        blackPawnsLegalMovesMap.Add(p.ID, new LegalMoveMap(p, null));
                    });
                    break;
            }
        }

        public void UpdateWhitePawnsLegalMoves(ChessBoardCell[,] cells)
        {
            foreach (var p in whitePawnsLegalMovesMap)
            {
                whitePawnsLegalMovesMap[p.Key].legalMoves = GetValidCells(p.Value.pawn.pieceType, p.Value.pawn.IsFirstMove, p.Value.pawn.owner, p.Value.pawn.cell.Index, cells);
            }
        }

        public void UpdateBlackPawnsLegalMoves(ChessBoardCell[,] cells)
        {
            foreach (var p in blackPawnsLegalMovesMap)
            {
                blackPawnsLegalMovesMap[p.Key].legalMoves = GetValidCells(p.Value.pawn.pieceType, p.Value.pawn.IsFirstMove, p.Value.pawn.owner, p.Value.pawn.cell.Index, cells);
            }
        }

        public List<ChessBoardCell> GetWhitePawnLegalMoves(PawnController pawn)
        {
            return whitePawnsLegalMovesMap[pawn.ID].legalMoves;
        }

        public List<ChessBoardCell> GetBlackPawnLegalMoves(PawnController pawn)
        {
            return blackPawnsLegalMovesMap[pawn.ID].legalMoves;
        }

        public List<ChessBoardCell> GetValidCells(PieceType pieceType, bool isFirstTurn, PlayerType owner, Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    return GetMovableCellsForPawn(owner, isFirstTurn, pawnCell, cells);

                case PieceType.Knight:
                    return GetMovableCellsForKnight(owner, pawnCell, cells);

                case PieceType.Bishop:
                    var movableCells = new List<ChessBoardCell>();
                    return GetMovableCellsForBishop(owner, pawnCell, cells, ref movableCells);

                case PieceType.Rook:
                    movableCells = new List<ChessBoardCell>();
                    return GetMovableCellsForRook(owner, pawnCell, cells, ref movableCells);
                case PieceType.Queen:
                    return GetMovableCellsForQueen(owner, pawnCell, cells);

                case PieceType.King:
                    return GetMovableCellsForKing(owner, pawnCell, cells);
                default: return new List<ChessBoardCell>();
            }
        }

        private List<ChessBoardCell> GetMovableCellsForPawn(PlayerType owner, bool isFirstTurn, Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            List<ChessBoardCell> movableCells = new List<ChessBoardCell>();

            if (cells == null)
            {
                return movableCells;
            }


            //Forward cell
            int forwardCells = (isFirstTurn) ? 2 : 1;

            for (int i = 0; i < forwardCells; i++)
            {
                if (pawnCell.x < ChessBoard.BOARD_COLS - 1 - i)
                {
                    if (cells[pawnCell.x + 1 + i, pawnCell.y].IsEmpty)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + i + 1, pawnCell.y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //Left diagonal cell
            if (pawnCell.x < ChessBoard.BOARD_COLS - 1 && pawnCell.y > 0)
            {
                if (!cells[pawnCell.x + 1, pawnCell.y - 1].IsEmpty)
                {
                    if (cells[pawnCell.x + 1, pawnCell.y - 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y - 1]);
                    }
                }
            }

            //Right diagonal cell
            if (pawnCell.x < ChessBoard.BOARD_COLS - 1 && pawnCell.y < ChessBoard.BOARD_ROWS - 1)
            {
                if (!cells[pawnCell.x + 1, pawnCell.y + 1].IsEmpty)
                {
                    if (cells[pawnCell.x + 1, pawnCell.y + 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y + 1]);
                    }
                }
            }

            return movableCells;
        }

        private List<ChessBoardCell> GetMovableCellsForKnight(PlayerType owner, Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            List<ChessBoardCell> movableCells = new List<ChessBoardCell>();

            if (cells == null)
            {
                return movableCells;
            }


            if (pawnCell.x < ChessBoard.BOARD_COLS - 2)
            {
                //Forward Left
                if (pawnCell.y > 0)
                {
                    if (cells[pawnCell.x + 2, pawnCell.y - 1].IsEmpty || cells[pawnCell.x + 2, pawnCell.y - 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 2, pawnCell.y - 1]);
                    }
                }
                //Forward Right
                if (pawnCell.y < ChessBoard.BOARD_ROWS - 1)
                {
                    if (cells[pawnCell.x + 2, pawnCell.y + 1].IsEmpty || cells[pawnCell.x + 2, pawnCell.y + 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 2, pawnCell.y + 1]);
                    }
                }
            }

            if (pawnCell.y > 1)
            {
                //Left Bottom
                if (pawnCell.x > 0)
                {
                    if (cells[pawnCell.x - 1, pawnCell.y - 2].IsEmpty || cells[pawnCell.x - 1, pawnCell.y - 2].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y - 2]);
                    }
                }
                //Left Top
                if (pawnCell.x < ChessBoard.BOARD_COLS - 1)
                {
                    if (cells[pawnCell.x + 1, pawnCell.y - 2].IsEmpty || cells[pawnCell.x + 1, pawnCell.y - 2].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y - 2]);
                    }
                }
            }

            if (pawnCell.y < ChessBoard.BOARD_ROWS - 2)
            {
                //Right Bottom
                if (pawnCell.x > 0)
                {
                    if (cells[pawnCell.x - 1, pawnCell.y + 2].IsEmpty || cells[pawnCell.x - 1, pawnCell.y + 2].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y + 2]);
                    }
                }
                //Right Top
                if (pawnCell.x < ChessBoard.BOARD_COLS - 1)
                {
                    if (cells[pawnCell.x + 1, pawnCell.y + 2].IsEmpty || cells[pawnCell.x + 1, pawnCell.y + 2].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y + 2]);
                    }
                }
            }

            if (pawnCell.x > 1)
            {
                //Bottom Left
                if (pawnCell.y > 0)
                {
                    if (cells[pawnCell.x - 2, pawnCell.y - 1].IsEmpty || cells[pawnCell.x - 2, pawnCell.y - 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 2, pawnCell.y - 1]);
                    }
                }
                //Bottom Right
                if (pawnCell.y < ChessBoard.BOARD_ROWS - 1)
                {
                    if (cells[pawnCell.x - 2, pawnCell.y + 1].IsEmpty || cells[pawnCell.x - 2, pawnCell.y + 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 2, pawnCell.y + 1]);
                    }
                }
            }
            return movableCells;
        }

        private void AddToMovableCells(List<ChessBoardCell> movableCells, ChessBoardCell cell)
        {
            if (!movableCells.Contains(cell))
            {
                movableCells.Add(cell);
            }
        }

        private List<ChessBoardCell> GetMovableCellsForBishop(PlayerType owner, Vector2Int pawnCell, ChessBoardCell[,] cells, ref List<ChessBoardCell> movableCells)
        {
            if(movableCells == null)
            {
                movableCells = new List<ChessBoardCell>();
            }

            if (cells == null)
            {
                return movableCells;
            }

            //Forward Right
            int i = pawnCell.x + 1;
            int j = pawnCell.y + 1;

            for (; i < ChessBoard.BOARD_COLS && j < ChessBoard.BOARD_ROWS; i++, j++)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            //Forward Left
            i = pawnCell.x + 1;
            j = pawnCell.y - 1;

            for (; i < ChessBoard.BOARD_COLS && j >= 0; i++, j--)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            //Backward Right
            i = pawnCell.x - 1;
            j = pawnCell.y + 1;

            for (; i >= 0 && j < ChessBoard.BOARD_ROWS; i--, j++)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            //Backward Left
            i = pawnCell.x - 1;
            j = pawnCell.y - 1;

            for (; i >= 0 && j >= 0; i--, j--)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            return movableCells;
        }
        
        private List<ChessBoardCell> GetMovableCellsForRook(PlayerType owner, Vector2Int pawnCell, ChessBoardCell[,] cells, ref List<ChessBoardCell> movableCells)
        {
            if (movableCells == null)
            {
                movableCells = new List<ChessBoardCell>();
            }

            if (cells == null)
            {
                return movableCells;
            }

            int i = pawnCell.x + 1;
            int j = pawnCell.y;

            //Forward
            for(; i < ChessBoard.BOARD_ROWS; i++)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i,j]);
                    }
                    break;
                }
            }

            //Backward
            i = pawnCell.x - 1;
            j = pawnCell.y;
            for (; i >= 0; i--)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            //Right
            i = pawnCell.x;
            j = pawnCell.y + 1;
            for (; j < ChessBoard.BOARD_COLS; j++)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            //Left
            i = pawnCell.x;
            j = pawnCell.y - 1;
            for (; j >= 0; j--)
            {
                if (cells[i, j].IsEmpty)
                {
                    AddToMovableCells(movableCells, cells[i, j]);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i, j]);
                    }
                    break;
                }
            }

            return movableCells;
        }
    
        private List<ChessBoardCell> GetMovableCellsForQueen(PlayerType owner, Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            List<ChessBoardCell> movableCells = new List<ChessBoardCell>();

            if(cells == null)
            {
                return movableCells;
            }

            GetMovableCellsForBishop(owner, pawnCell, cells, ref movableCells);
            GetMovableCellsForRook(owner, pawnCell, cells, ref movableCells);

            return movableCells;
        }

        private List<ChessBoardCell> GetMovableCellsForKing(PlayerType owner, Vector2Int pawnCell, ChessBoardCell[,] cells)
        {
            List<ChessBoardCell> movableCells = new List<ChessBoardCell>();

            if(cells == null)
            {
                return movableCells;
            }

            //Forward
            if (pawnCell.x < ChessBoard.BOARD_ROWS - 1)
            {
                if (cells[pawnCell.x + 1, pawnCell.y].IsEmpty || (cells[pawnCell.x + 1, pawnCell.y].pawn && cells[pawnCell.x + 1, pawnCell.y].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y]);
                }
            }

            //Backward
            if (pawnCell.x > 0)
            {
                if (cells[pawnCell.x - 1, pawnCell.y].IsEmpty || (cells[pawnCell.x - 1, pawnCell.y].pawn && cells[pawnCell.x - 1, pawnCell.y].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x - 1 , pawnCell.y]);
                }
            }

            //Right
            if (pawnCell.y < ChessBoard.BOARD_COLS - 1)
            {
                if (cells[pawnCell.x, pawnCell.y + 1].IsEmpty || (cells[pawnCell.x, pawnCell.y + 1].pawn && cells[pawnCell.x, pawnCell.y + 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x, pawnCell.y + 1]);
                }
            }

            //Left
            if (pawnCell.y > 0)
            {
                if (cells[pawnCell.x, pawnCell.y - 1].IsEmpty || (cells[pawnCell.x, pawnCell.y - 1].pawn && cells[pawnCell.x, pawnCell.y - 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x, pawnCell.y - 1]);
                }
            }

            //Left Forward
            if (pawnCell.y > 0 && pawnCell.x < ChessBoard.BOARD_ROWS - 1)
            {
                if (cells[pawnCell.x + 1, pawnCell.y - 1].IsEmpty || (cells[pawnCell.x + 1, pawnCell.y - 1].pawn && cells[pawnCell.x + 1, pawnCell.y - 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y - 1]);
                }
            }

            //Right Forward
            if (pawnCell.y < ChessBoard.BOARD_COLS - 1 && pawnCell.x < ChessBoard.BOARD_ROWS - 1)
            {
                if (cells[pawnCell.x + 1, pawnCell.y + 1].IsEmpty || (cells[pawnCell.x + 1, pawnCell.y + 1].pawn && cells[pawnCell.x + 1, pawnCell.y + 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y + 1]);
                }
            }

            //Left Backward
            if (pawnCell.y > 0 && pawnCell.x > 0)
            {
                if (cells[pawnCell.x - 1, pawnCell.y - 1].IsEmpty || (cells[pawnCell.x - 1, pawnCell.y - 1].pawn && cells[pawnCell.x - 1, pawnCell.y - 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y - 1]);
                }
            }

            //Right Backward
            if (pawnCell.y < ChessBoard.BOARD_COLS - 1 && pawnCell.x > 0)
            {
                if (cells[pawnCell.x - 1, pawnCell.y + 1].IsEmpty || (cells[pawnCell.x - 1, pawnCell.y + 1].pawn && cells[pawnCell.x - 1, pawnCell.y + 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y + 1]);
                }
            }

            return movableCells;
        }
    }
}