using Anvarat.Utils;
using CustomChess.Board;
using CustomChess.Pieces.Pawn;
using CustomChess.Utils;

namespace CustomChess.Base
{
    public class DataHandler : Singleton<DataHandler>
    {
        [ReadOnly]
        public PlayerType CurrentTurn;
        [ReadOnly]
        public PawnController SelectedPawn;
    }
}