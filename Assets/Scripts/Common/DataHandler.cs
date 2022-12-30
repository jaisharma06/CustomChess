using CustomChess.Pieces.Pawn;
using CustomChess.Utils;

namespace CustomChess.Base
{
    public class DataHandler : Singleton<DataHandler>
    {
        public PlayerType CurrentTurn { get; set; }
        public PawnController SelectedPawn { get; set; }
    }
}