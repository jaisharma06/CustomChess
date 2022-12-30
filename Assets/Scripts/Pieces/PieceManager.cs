using CustomChess.Base;
using CustomChess.Board;
using CustomChess.Pieces.Pawn;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomChess.Pieces
{
    [Serializable]
    public class PawnPositionMap 
    {
        public PieceType pieceType;
        public Vector2 cellIndex;
    }

    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private List<PawnController> m_pawns;
        [SerializeField] private List<PawnPositionMap> m_whitePawnPositions;
        [SerializeField] private List<PawnPositionMap> m_blackPawnPositions;

        [Header("Materials")]
        [SerializeField] private Material m_blackMaterial;
        [SerializeField] private Material m_whiteMaterial;

        [Header("Parents")]
        [SerializeField] private Transform m_whitePieceParent;
        [SerializeField] private Transform m_blackPieceParent;

        private List<PawnController> _playerPawns;
        private List<PawnController> _opponentPawns;

        public void PlacePieces(ChessBoardCell[,] cells)
        {
            PlacePlayerPawns(cells);
            PlaceOpponentPawns(cells);
        }

        private void PlacePlayerPawns(ChessBoardCell[,] cells)
        {
            _playerPawns = new List<PawnController>();
            for(int i = 0; i < m_whitePawnPositions.Count; i++)
            {
                var piecePrefab = m_pawns.FirstOrDefault(p => p.pieceType == m_whitePawnPositions[i].pieceType);
                var piece = Instantiate(piecePrefab, m_whitePieceParent);
                var cellIndex = m_whitePawnPositions[i].cellIndex;
                var cell = cells[Mathf.RoundToInt(cellIndex.x), Mathf.RoundToInt(cellIndex.y)];
                piece.SetPosition(cell.transform.position.x, cell.transform.position.z);
                piece.SetOwner(PlayerType.Player1);
                piece.SetMaterial(m_whiteMaterial);
                cell.pawn = piece;
                _playerPawns.Add(piece);
            }
        }

        private void PlaceOpponentPawns(ChessBoardCell[,] cells)
        {
            _opponentPawns = new List<PawnController>();
            for (int i = 0; i < m_blackPawnPositions.Count; i++)
            {
                var piecePrefab = m_pawns.FirstOrDefault(p => p.pieceType == m_blackPawnPositions[i].pieceType);
                var piece = Instantiate(piecePrefab, m_blackPieceParent);
                var cellIndex = m_blackPawnPositions[i].cellIndex;
                var cell = cells[Mathf.RoundToInt(cellIndex.x), Mathf.RoundToInt(cellIndex.y)];
                piece.SetPosition(cell.transform.position.x, cell.transform.position.z);
                piece.SetOwner(PlayerType.AI);
                piece.SetMaterial(m_blackMaterial);
                cell.pawn = piece;
                _opponentPawns.Add(piece);
            }
        }
    }
}