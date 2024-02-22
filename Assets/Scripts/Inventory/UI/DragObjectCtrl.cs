using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObjectCtrl : MonoBehaviour//, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Vector3 _initialPosition;

    [SerializeField] private RectTransform _childTransform;

    [SerializeField] private RectTransform m_DraggingPlane;

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        _childTransform = GetComponentInChildren<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = eventData.pointerEnter.transform as RectTransform;

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            _childTransform.position = globalMousePos;
            _childTransform.rotation = m_DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }*/

    private void Awake()
    {
        m_DraggingPlane = transform.parent.GetComponent<RectTransform>();
    }

   private void OnMouseDown()
    {
        _childTransform = GetComponentInChildren<RectTransform>();
        _initialPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        /*RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "InventoryItem")
            {

            }
        }*/
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, transform.position, Camera.main, out globalMousePos))
        {
            transform.position = globalMousePos;
            transform.rotation = m_DraggingPlane.rotation;
        }
    }

    private void OnMouseUp()
    {
        
    }
}
