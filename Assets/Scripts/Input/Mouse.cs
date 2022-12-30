using UnityEngine;

namespace CustomChess.MouseInput
{
    public class Mouse : MonoBehaviour
    {
        private Camera _mainCamera;

        private Ray _ray;
        private RaycastHit _hit;
        private RaycastHit _lastHit;

        private Collider _mouseDownObject;

        private IMouseEnter _mouseEnter;
        private IMouseExit _mouseExit;
        private IMouseDown _mouseDown;
        private IMouseUp _mouseUp;
        private IMouseClick _mouseClick;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            CheckForMouseOverlap();

            if (Input.GetMouseButtonDown(0))
            {
                if (_lastHit.collider)
                {
                    _mouseDown = _lastHit.collider.GetComponent<IMouseDown>();
                    if (_mouseDown != null)
                    {
                        _mouseDown.OnMouseDown();
                    }
                    _mouseDownObject = _lastHit.collider;
                    Debug.Log($"Mouse Downed on: {_mouseDownObject.name}");
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_lastHit.collider)
                {
                    _mouseUp = _lastHit.collider.GetComponent<IMouseUp>();
                    if (_mouseUp != null)
                    {
                        _mouseUp.OnMouseUp();
                    }

                    Debug.Log($"Mouse Uped on: {_mouseDownObject.name}, {_lastHit.collider.name}");
                    if (_mouseDownObject == _lastHit.collider)
                    {
                        _mouseClick = _mouseDownObject.GetComponent<IMouseClick>();
                        _mouseClick.OnMouseClick();
                    }
                }
            }

        }

        private void CheckForMouseOverlap()
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 100))
            {
                if (!_lastHit.collider || _lastHit.collider != _hit.collider)
                {
                    if (_lastHit.collider)
                    {
                        _mouseExit = _lastHit.collider.GetComponent<IMouseExit>();
                        if (_mouseExit != null) _mouseExit.OnMouseExit();

                        _mouseEnter = _hit.collider.GetComponent<IMouseEnter>();
                        if (_mouseEnter != null) _mouseEnter.OnMouseEnter();

                        _lastHit = _hit;
                        _mouseEnter = null;
                        _mouseExit = null;
                    }
                }
            }
            else
            {
                if (_lastHit.collider)
                {
                    _mouseEnter = _lastHit.collider.GetComponent<IMouseEnter>();
                    if (_mouseEnter != null) _mouseEnter.OnMouseEnter();

                    _lastHit = _hit;
                    _mouseEnter = null;
                }
            }
        }
    }
}