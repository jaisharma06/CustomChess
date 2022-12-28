using System.Collections.Generic;
using UnityEngine;

namespace CustomChess.ChessBoard
{
    public class ChessBoard : MonoBehaviour
    {
        [SerializeField] private ChessBoardCell m_cellPrefab;
        [SerializeField] private Vector2 m_firstCellPos;
        [SerializeField] private Vector2 m_cellPadding;

        private const int BOARD_ROWS = 8;
        private const int BOARD_COLS = 8;

        private ChessBoardCell[ , ] _cells = new ChessBoardCell[BOARD_ROWS, BOARD_COLS];

        private Vector2 _workSpace = Vector2.zero;

        private void Awake()
        {
            InitializeCells();
        }

        private void InitializeCells()
        {
            for(int i =0; i < BOARD_ROWS; i++)
            {
                for (int j = 0; j < BOARD_COLS; j++)
                {
                    var cell = Instantiate(m_cellPrefab, transform);
                    cell.name = $"Cell_{i}_{j}";
                    _workSpace.Set(m_firstCellPos.x + (j * m_cellPadding.x), m_firstCellPos.y - (i * m_cellPadding.y));
                    cell.SetPos(_workSpace.x, _workSpace.y);
                    cell.Deactivate();
                    _cells[i, j] = cell;
                }
            }
        }
    }
}