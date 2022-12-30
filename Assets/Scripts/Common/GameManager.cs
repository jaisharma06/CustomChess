using CustomChess.Board;
using CustomChess.Pieces;
using UnityEngine;

namespace CustomChess.Base
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ChessBoard m_board;
        [SerializeField] private PieceManager m_pieceManager;

        private void Awake()
        {
            PrepareBoard();
        }

        private void PrepareBoard()
        {
            m_board.InitializeCells();
            m_pieceManager.PlacePieces(m_board.cells);

            SwitchTurn(PlayerType.Player1);
        }

        public void SwitchTurn(PlayerType turn)
        {
            DataHandler.instance.CurrentTurn = turn;
        }
    }
}

