using System;
using UnityEngine;

namespace CustomChess.ChessBoard
{
    public class ChessBoardCell : MonoBehaviour
    {
        private Vector3 _workspace;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void SetPos(float x, float y)
        {
            _workspace.Set(x, y, transform.localPosition.z);
            transform.localPosition = _workspace;
        }

        public void Activate()
        {
            _renderer.enabled = true;
        }

        public void Deactivate()
        {
            _renderer.enabled = false;
        }
    }
}