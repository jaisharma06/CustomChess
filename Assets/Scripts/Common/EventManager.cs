using CustomChess.Board;
using System;

namespace CustomChess.Events
{
    public static class EventManager
    {
        public static Action<ChessBoardCell, bool> PawnSelected;
        public static Action PawnStartedMoving;
        public static Action PawnStoppedMoving;
    }
}