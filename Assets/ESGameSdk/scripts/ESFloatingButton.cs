
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ESFloatingButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    , IPointerClickHandler
{
    public float maxTimeToReposition = 1;
    public float timeToHide = 1;
    public float hideDuration = 1;
    private float cTime = 0;
    public UnityEvent callback;
    private Vector2 lastMousePosition;
    private bool dragging;
    private TweenerCore<Vector3, Vector3, VectorOptions> currentTween;
    

    private void OnEnable()
    {
        OnEndDrag(null);
    }

    /// <summary>
    /// This method will be called on the start of the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        lastMousePosition = eventData.position;
        if (currentTween != null)
        {
            currentTween.Kill();
        }
        dragging = true;
        cTime = 0;
    }

    /// <summary>
    /// This method will be called during the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rect.position;
        rect.position = newPosition;
        var visibleCount = checkVisibleCornerCount(rect);
                    Debug.Log("visibleCount " + visibleCount);
        Debug.Log("dragend " + gameObject.transform.position);
        if (visibleCount<= 2 && (gameObject.transform.position.x <= 0 || gameObject.transform.position.x >= Screen.width 
                || gameObject.transform.position.y <= 0 || gameObject.transform.position.y >= Screen.height 
            ))
        {
            rect.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }

    /// <summary>
    /// This method will be called at the end of mouse drag
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        Debug.Log("dragend "+gameObject.transform.position );
        moveToCorner(1, () => {
            cTime = timeToHide;
        });
    }

    private void Update()
    {
        if(cTime > 0)
        {
            cTime -= Time.deltaTime;
            if(cTime <= 0)
            {
                moveToInOrOut(0);
            }
        }
    }

    internal bool showFullDisplayIfNeed()
    {
        Debug.Log("showFullDisplayIfNeed x " + gameObject.transform.position.x);
        if (gameObject.transform.position.x < gameObject.GetComponent<RectTransform>().rect.width / 2
            || gameObject.transform.position.x > Screen.width -gameObject.GetComponent<RectTransform>().rect.width / 2
            )
        {
            Debug.Log("showFullDisplayIfNeed "+true);
            moveToInOrOut(1);
            return true;
        }
        return false;
    }

    private void moveToCorner(float factor,TweenCallback callback)
    {
        float x = 0;
        if (gameObject.transform.position.x <= Screen.width / 2)
        {
            x = gameObject.GetComponent<RectTransform>().rect.width / 2 * factor;
        }
        else if (gameObject.transform.position.x >= Screen.width / 2)
        {
            x = Screen.width - gameObject.GetComponent<RectTransform>().rect.width / 2 * factor;
        }
        float time = Mathf.Abs( x-gameObject.transform.position.x) / Screen.width / 2 * maxTimeToReposition;
        if (time > 0)
        {
            this.currentTween = moveTo(x, time,callback);
        }
    }

    private void moveToInOrOut(float factor)
    {
        float x = 0;
        if (gameObject.transform.position.x <= Screen.width / 2)
        {
            x = gameObject.GetComponent<RectTransform>().rect.width / 2 * factor;
        }
        else if (gameObject.transform.position.x >= Screen.width / 2)
        {
            x = Screen.width - gameObject.GetComponent<RectTransform>().rect.width / 2 * factor;
        }
        this.currentTween = moveTo(x, hideDuration, ()=> {
            if(factor == 1)
            {
                cTime = timeToHide;
            }
        });
    }

    private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> moveTo(float x,float time,TweenCallback callback)
    {
        DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> rs = gameObject.transform.DOMoveX(x, time).SetEase(Ease.OutBack);
        if(callback!=null)
        rs.OnComplete(callback);
        return rs;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick " + dragging);
        if (!dragging)
            callback?.Invoke();
    }
    /// <summary>
    /// This methods will check is the rect transform is inside the screen or not
    /// </summary>
    /// <param name="rectTransform">Rect Trasform</param>
    /// <returns></returns>
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        return checkVisibleCornerCount(rectTransform) == 4;
    }


    /// <summary>
    /// This methods will check is the rect transform is inside the screen or not
    /// </summary>
    /// <param name="rectTransform">Rect Trasform</param>
    /// <returns></returns>
    private int checkVisibleCornerCount(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
       
        return visibleCorners;
    }
}