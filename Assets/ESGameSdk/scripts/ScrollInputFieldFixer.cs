using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InputField))]
public class ScrollInputFieldFixer : MonoBehaviour, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IPointerClickHandler,IPointerUpHandler,IPointerExitHandler
{
    private ScrollRect _scrollRect = null;
    private InputField _input = null;
    private bool _isDragging = false;


    private void Start()
    {
        _scrollRect = GetComponentInParent<ScrollRect>();
        _input = GetComponent<InputField>();
        _input.DeactivateInputField();
        _input.enabled = true ;
    }


    public void OnBeginDrag(PointerEventData data)
    {
        if (_scrollRect != null && _input != null)
        {
            _isDragging = true;
            _input.DeactivateInputField();
            _input.enabled = false;
            _scrollRect.SendMessage("OnBeginDrag", data);
        }
    }


    public void OnEndDrag(PointerEventData data)
    {
        if (_scrollRect != null && _input != null)
        {
            _isDragging = false;
            _scrollRect.SendMessage("OnEndDrag", data);
        }
    }


    public void OnDrag(PointerEventData data)
    {
        if (_scrollRect != null && _input != null)
        {
            _scrollRect.SendMessage("OnDrag", data);
        }
    }


    public void OnPointerClick(PointerEventData data)
    {
        if (_scrollRect != null && _input != null)
        {
            if (!_isDragging && !data.dragging)
            {
                _input.enabled = true;
                _input.ActivateInputField();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
