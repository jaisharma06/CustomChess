using CustomChess.Board;
using CustomChess.ConnectionLibrary;
using CustomChess.Events;
using CustomChess.Pieces;
using UnityEngine;

namespace CustomChess.Base
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ChessBoard m_board;
        [SerializeField] private PieceManager m_pieceManager;

        private PlayerType _lastPlayerTurn;

        public SocketHandler socketHandler { get; private set; }

        private void OnEnable()
        {
            EventManager.PawnStartedMoving += OnPawnMovementStart;
            EventManager.PawnStoppedMoving += OnPawnMovementEnd;
        }

        private void Awake()
        {
            socketHandler = new SocketHandler("http://192.168.1.6:3000");
            PrepareBoard();
        }

        private void OnDisable()
        {
            EventManager.PawnStartedMoving -= OnPawnMovementStart;
            EventManager.PawnStoppedMoving -= OnPawnMovementEnd;
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

        private void OnPawnMovementStart()
        {
            _lastPlayerTurn = DataHandler.instance.CurrentTurn;
            SwitchTurn(PlayerType.None);
        }

        private void OnPawnMovementEnd()
        {
            switch (_lastPlayerTurn)
            {
                case PlayerType.Player1:
                    if (DataHandler.instance.isOpponentAI)
                    {
                        SwitchTurn(PlayerType.Player1); // AI
                    }
                    else
                    {
                        SwitchTurn(PlayerType.Player1); //Player2
                    }
                    break;

                case PlayerType.Player2:
                case PlayerType.AI:
                    SwitchTurn(PlayerType.Player1);
                    break;
            }
            
        }
    }
}

