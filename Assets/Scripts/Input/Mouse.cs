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

        private IMousePointerEnter _mouseEnter;
        private IMousePointerExit _mouseExit;
        private IMousePointerDown _mouseDown;
        private IMousePointerUp _mouseUp;
        private IMousePointerClick _mouseClick;

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
                    _mouseDown = _lastHit.collider.GetComponent<IMousePointerDown>();
                    if (_mouseDown != null)
                    {
                        _mouseDown.OnMousePointerDown();
                        _mouseDown = null;
                    }
                    _mouseDownObject = _lastHit.collider;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_lastHit.collider)
                {
                    _mouseUp = _lastHit.collider.GetComponent<IMousePointerUp>();
                    if (_mouseUp != null)
                    {
                        _mouseUp.OnMousePointerUp();
                        _mouseUp = null;
                    }

                    if (_mouseDownObject == _lastHit.collider)
                    {
                        _mouseClick = _mouseDownObject.GetComponent<IMousePointerClick>();
                        _mouseClick.OnMousePointerClick();
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
                        _mouseExit = _lastHit.collider.GetComponent<IMousePointerExit>();
                        if (_mouseExit != null) _mouseExit.OnMousePointerExit();
                    }


                    _mouseEnter = _hit.collider.GetComponent<IMousePointerEnter>();
                    if (_mouseEnter != null) _mouseEnter.OnMousePointerEnter();

                    _lastHit = _hit;
                    _mouseEnter = null;
                    _mouseExit = null;
                }
            }
            else
            {
                if (_lastHit.collider)
                {
                    _mouseExit = _lastHit.collider.GetComponent<IMousePointerExit>();
                    if (_mouseExit != null) _mouseExit.OnMousePointerExit();

                    _lastHit = _hit;
                    _mouseExit = null;
                }
            }
        }
    }
}