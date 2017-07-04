using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    #region Layer Masks

    private const int AllLayerMask = int.MaxValue;


    [SerializeField]
    private LayerMask _groundLayerMask;

    [SerializeField]
    private LayerMask _workerAreaMask;

    #endregion

    private GraphicRaycaster uiRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> uiRaycastResults;

    private Vector3 _currentMousePosition;
    private GameObject _objectUnderMouse;

    #region Dragging Fields

    private bool _isDragging = false;
    // How much to wait after a click is registered, before we consider this a "drag" (pun intended)
    private const float TimeBeforeDrag = 0.06f;

    private float _draggingTimer = 0.0f;
    //Because the mouse moves independently of frame-rate, its possible to "lose" objects being dragged, that's why we cache them.
    private GameObject _objectBeingDragged;

    private Vector3 _dragStartPos;

    #endregion




    #region Actions

    public static Action<GameObject> onHover;
    public static Action<GameObject> onClick;
    public static Action<GameObject, Vector3, Vector3, GameObject> onDrag;
    public static Action<GameObject, GameObject> onRelease;

    #endregion

    private void Awake ()
    {
        uiRaycaster = GameObject.Find ("Canvas").GetComponent<GraphicRaycaster> ();
        pointerEventData = new PointerEventData (null);


    }
    private void Update ()
    {
        _currentMousePosition = GetCurrentMousePosition ();
        _objectUnderMouse = GetObjectUnderMouse (_workerAreaMask);
        if (_objectUnderMouse == null)
            _objectUnderMouse = GetObjectUnderMouse (AllLayerMask);
        if (_objectUnderMouse == null)
            _objectUnderMouse = this.gameObject;

        if (_isDragging && onDrag != null)
        {
            onDrag (_objectBeingDragged, _dragStartPos, _currentMousePosition, GetObjectUnderMouse (_groundLayerMask));
        }
        else if (onHover != null)
        {
            onHover (_objectUnderMouse);
        }

        if (Input.GetMouseButtonUp (0))
        {
            _isDragging = false;
            if (onRelease != null)
                onRelease (_objectBeingDragged, GetObjectUnderMouse (_groundLayerMask));

            _objectBeingDragged = gameObject;
            _isDragging = false;
        }

        // Ignore the UI layer.
        pointerEventData.position = Input.mousePosition;
        uiRaycastResults = new List<RaycastResult> ();
        uiRaycaster.Raycast (pointerEventData, uiRaycastResults);
        if (uiRaycastResults.Count > 0)
        {
            return;
        }


        if (Input.GetMouseButtonDown (0) && onClick != null)
        {
            _dragStartPos = _currentMousePosition;
            _objectBeingDragged = _objectUnderMouse;
            _draggingTimer = 0.0f;
            onClick (_objectUnderMouse);
        }
        else if (Input.GetMouseButton (0) && _isDragging == false)
        {
            _draggingTimer += Time.deltaTime;
            if (_draggingTimer > TimeBeforeDrag)
                _isDragging = true;
        }


    }


    #region Helper Methods

    // Cast a ray from the mouse to the "ground" layer and returns the point of the hit to get the mouse position in 3D space. 
    // If nothing is hit returns the last valid position.
    private Vector3 GetCurrentMousePosition ()
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        return Physics.Raycast (ray, out hit, Mathf.Infinity, _groundLayerMask) ? hit.point : _currentMousePosition;
    }

    // Cast a ray and returns the first object hit.
    // If nothing is hit returns the gameObject this script is attached to (all code paths must return a game object, can't be null).
    private GameObject GetObjectUnderMouse (LayerMask mask)
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast (ray, out hit, Mathf.Infinity, mask);

        //Debug.DrawRay (ray.origin, ray.direction * Mathf.Infinity, Color.yellow);

        return (hit.collider != null) ? hit.collider.gameObject : null;
    }

    #endregion

}