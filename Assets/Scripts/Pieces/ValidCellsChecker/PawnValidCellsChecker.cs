using CustomChess.Board;
using CustomChess.Pieces.Pawn;
using System.Collections.Generic;
using System.Linq;
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
                if(whitePawnsLegalMovesMap[p.Key].legalMoves != null)
                {
                    whitePawnsLegalMovesMap[p.Key].legalMoves.ForEach(c => c.LegalFor.Remove(PlayerType.Player1));
                }
                whitePawnsLegalMovesMap[p.Key].legalMoves = GetValidCells(p.Value.pawn.pieceType, p.Value.pawn.IsFirstMove, p.Value.pawn.owner, p.Value.pawn.cell.Index, cells);
            }

            var king = IsCheckMate();
            if (king)
            {
                Debug.Log($"Check Mate __{king.owner}__");
            }
        }

        public void UpdateBlackPawnsLegalMoves(ChessBoardCell[,] cells, PlayerType owner)
        {
            foreach (var p in blackPawnsLegalMovesMap)
            {
                if (blackPawnsLegalMovesMap[p.Key].legalMoves != null)
                {
                    blackPawnsLegalMovesMap[p.Key].legalMoves.ForEach(c => c.LegalFor.Remove(owner));
                }
                blackPawnsLegalMovesMap[p.Key].legalMoves = GetValidCells(p.Value.pawn.pieceType, p.Value.pawn.IsFirstMove, owner, p.Value.pawn.cell.Index, cells);
            }

            var king = IsCheckMate();
            if (king)
            {
                Debug.Log($"Check Mate __{king.owner}__");
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
            switch (owner)
            {
                case PlayerType.Player1:
                    return GetMovableCellsForWhitePawns(owner, isFirstTurn, pawnCell, cells);
                default:
                    return GetMovableCellsForBlackPawns(owner, isFirstTurn, pawnCell, cells);

            }
        }

        private List<ChessBoardCell> GetMovableCellsForWhitePawns(PlayerType owner, bool isFirstTurn, Vector2Int pawnCell, ChessBoardCell[,] cells)
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
                        AddToMovableCells(movableCells, cells[pawnCell.x + i + 1, pawnCell.y], owner);
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
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y - 1], owner);
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
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y + 1], owner);
                    }
                }
            }

            return movableCells;
        }

        private List<ChessBoardCell> GetMovableCellsForBlackPawns(PlayerType owner, bool isFirstTurn, Vector2Int pawnCell, ChessBoardCell[,] cells)
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
                if (pawnCell.x  - (i + 1) >= 0)
                {
                    if (cells[pawnCell.x - (1 + i), pawnCell.y].IsEmpty)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - (i + 1), pawnCell.y], owner);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //Left diagonal cell
            if (pawnCell.x - 1 >= 0 && pawnCell.y > 0)
            {
                if (!cells[pawnCell.x - 1, pawnCell.y - 1].IsEmpty)
                {
                    if (cells[pawnCell.x - 1, pawnCell.y - 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y - 1], owner);
                    }
                }
            }

            //Right diagonal cell
            if (pawnCell.x - 1 >= 0 && pawnCell.y < ChessBoard.BOARD_ROWS - 1)
            {
                if (!cells[pawnCell.x - 1, pawnCell.y + 1].IsEmpty)
                {
                    if (cells[pawnCell.x - 1, pawnCell.y + 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y + 1], owner);
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
                        AddToMovableCells(movableCells, cells[pawnCell.x + 2, pawnCell.y - 1], owner);
                    }
                }
                //Forward Right
                if (pawnCell.y < ChessBoard.BOARD_ROWS - 1)
                {
                    if (cells[pawnCell.x + 2, pawnCell.y + 1].IsEmpty || cells[pawnCell.x + 2, pawnCell.y + 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 2, pawnCell.y + 1], owner);
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
                        AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y - 2], owner);
                    }
                }
                //Left Top
                if (pawnCell.x < ChessBoard.BOARD_COLS - 1)
                {
                    if (cells[pawnCell.x + 1, pawnCell.y - 2].IsEmpty || cells[pawnCell.x + 1, pawnCell.y - 2].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y - 2], owner);
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
                        AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y + 2], owner);
                    }
                }
                //Right Top
                if (pawnCell.x < ChessBoard.BOARD_COLS - 1)
                {
                    if (cells[pawnCell.x + 1, pawnCell.y + 2].IsEmpty || cells[pawnCell.x + 1, pawnCell.y + 2].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y + 2], owner);
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
                        AddToMovableCells(movableCells, cells[pawnCell.x - 2, pawnCell.y - 1], owner);
                    }
                }
                //Bottom Right
                if (pawnCell.y < ChessBoard.BOARD_ROWS - 1)
                {
                    if (cells[pawnCell.x - 2, pawnCell.y + 1].IsEmpty || cells[pawnCell.x - 2, pawnCell.y + 1].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[pawnCell.x - 2, pawnCell.y + 1], owner);
                    }
                }
            }
            return movableCells;
        }

        private void AddToMovableCells(List<ChessBoardCell> movableCells, ChessBoardCell cell, PlayerType owner)
        {
            if (!movableCells.Contains(cell))
            {
                if (!cell.LegalFor.Contains(owner))
                {
                    cell.LegalFor.Add(owner);
                }
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn != null && cells[i, j].pawn.owner != owner)
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i,j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[i, j], owner);
                }
                else
                {
                    if (cells[i, j].pawn && (cells[i, j].pawn.owner != owner))
                    {
                        AddToMovableCells(movableCells, cells[i, j], owner);
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
                    AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y], owner);
                }
            }

            //Backward
            if (pawnCell.x > 0)
            {
                if (cells[pawnCell.x - 1, pawnCell.y].IsEmpty || (cells[pawnCell.x - 1, pawnCell.y].pawn && cells[pawnCell.x - 1, pawnCell.y].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x - 1 , pawnCell.y], owner);
                }
            }

            //Right
            if (pawnCell.y < ChessBoard.BOARD_COLS - 1)
            {
                if (cells[pawnCell.x, pawnCell.y + 1].IsEmpty || (cells[pawnCell.x, pawnCell.y + 1].pawn && cells[pawnCell.x, pawnCell.y + 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x, pawnCell.y + 1], owner);
                }
            }

            //Left
            if (pawnCell.y > 0)
            {
                if (cells[pawnCell.x, pawnCell.y - 1].IsEmpty || (cells[pawnCell.x, pawnCell.y - 1].pawn && cells[pawnCell.x, pawnCell.y - 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x, pawnCell.y - 1], owner);
                }
            }

            //Left Forward
            if (pawnCell.y > 0 && pawnCell.x < ChessBoard.BOARD_ROWS - 1)
            {
                if (cells[pawnCell.x + 1, pawnCell.y - 1].IsEmpty || (cells[pawnCell.x + 1, pawnCell.y - 1].pawn && cells[pawnCell.x + 1, pawnCell.y - 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y - 1], owner);
                }
            }

            //Right Forward
            if (pawnCell.y < ChessBoard.BOARD_COLS - 1 && pawnCell.x < ChessBoard.BOARD_ROWS - 1)
            {
                if (cells[pawnCell.x + 1, pawnCell.y + 1].IsEmpty || (cells[pawnCell.x + 1, pawnCell.y + 1].pawn && cells[pawnCell.x + 1, pawnCell.y + 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x + 1, pawnCell.y + 1], owner);
                }
            }

            //Left Backward
            if (pawnCell.y > 0 && pawnCell.x > 0)
            {
                if (cells[pawnCell.x - 1, pawnCell.y - 1].IsEmpty || (cells[pawnCell.x - 1, pawnCell.y - 1].pawn && cells[pawnCell.x - 1, pawnCell.y - 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y - 1], owner);
                }
            }

            //Right Backward
            if (pawnCell.y < ChessBoard.BOARD_COLS - 1 && pawnCell.x > 0)
            {
                if (cells[pawnCell.x - 1, pawnCell.y + 1].IsEmpty || (cells[pawnCell.x - 1, pawnCell.y + 1].pawn && cells[pawnCell.x - 1, pawnCell.y + 1].pawn.owner != owner))
                {
                    AddToMovableCells(movableCells, cells[pawnCell.x - 1, pawnCell.y + 1], owner);
                }
            }

            return movableCells;
        }

        private PawnController IsCheckMate()
        {
            var whiteKingLegalMovesMap = whitePawnsLegalMovesMap.FirstOrDefault(p => p.Value.pawn.pieceType == PieceType.King);
            if(whiteKingLegalMovesMap.Value != null)
            {
                //Condition for white check mate
            }

            var blackKingLegalMovesMap = blackPawnsLegalMovesMap.FirstOrDefault(p => p.Value.pawn.pieceType == PieceType.King);
            if (blackKingLegalMovesMap.Value != null)
            {
                //Condition for black check mate
            }
            return null;
        }
    }
}