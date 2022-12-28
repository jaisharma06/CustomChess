using CustomChess.Base;
using UnityEngine;

namespace CustomChess.Pieces
{
    [CreateAssetMenu(fileName = "PieceData", menuName = "Custom Chess/Data/Piece Data")]
    public class PieceData : Data
    {
        public float moveSpeed = 1f;

        [Header("Colors")]
        public Color hoverColor = Color.yellow;
        public Color originalColor = Color.white;
        public Color selectedColor;
    }
}