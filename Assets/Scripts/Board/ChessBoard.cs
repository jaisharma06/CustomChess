using CustomChess.Base;
using CustomChess.Events;
using CustomChess.Pieces;
using System.Collections.Generic;
using UnityEngine;

namespace CustomChess.Board
{
    public class ChessBoard : MonoBehaviour
    {
        [SerializeField] private PieceManager m_pieceManager;
        [SerializeField] private ChessBoardCell m_cellPrefab;
        [SerializeField] private Vector2 m_firstCellPos;
        [SerializeField] private Vector2 m_cellPadding;
        [SerializeField] private PawnValidCellsChecker m_validCellsChecker;

        private Vector2 _workSpace = Vector2.zero;

        public const int BOARD_ROWS = 8;
        public const int BOARD_COLS = 8;

        private ChessBoardCell[,] _cells = new ChessBoardCell[BOARD_ROWS, BOARD_COLS];

        public ChessBoardCell[,] cells { get => _cells; }
        private List<ChessBoardCell> _activatedCells = new List<ChessBoardCell>();

        private void OnEnable()
        {
            EventManager.PawnSelected += OnPawnSelected;
        }

        private void OnDisable()
        {
            EventManager.PawnSelected -= OnPawnSelected;
        }

        public void InitializeCells()
        {
            for(int i =0; i < BOARD_ROWS; i++)
            {
                for (int j = 0; j < BOARD_COLS; j++)
                {
                    var cell = Instantiate(m_cellPrefab, transform);
                    _workSpace.Set(m_firstCellPos.x + (j * m_cellPadding.x), m_firstCellPos.y - (i * m_cellPadding.y));
                    cell.name = $"Cell_{i}_{j}";
                    cell.Index.Set(i, j);
                    cell.SetPos(_workSpace.x, _workSpace.y);
                    cell.Deactivate();
                    _cells[i, j] = cell;
                }
            }
        }

        public void OnPawnSelected(ChessBoardCell cell, bool onlyDeactivate)
        {
            _activatedCells?.ForEach(c => c.Deactivate());

            if (onlyDeactivate)
            {
                _activatedCells.Clear();
                return;
            }

            m_validCellsChecker.UpdateWhitePawnsLegalMoves(_cells);
            _activatedCells = m_validCellsChecker.GetWhitePawnLegalMoves(cell.pawn);
            _activatedCells.ForEach(c => c.Activate());
        }
    }
}