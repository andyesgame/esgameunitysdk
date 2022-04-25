using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ESCoslapableView : MonoBehaviour
{
    private bool mState = false;
    
    public bool state {
        get { return mState; }
        set { setState(value); }
    }
    public float duration = 0.3f;
    private void setState(bool value)
    {
        mState = value;
        validateState();
    }

    public float spacing = 10;
    private List<Transform> items;
    private Vector3 tmp = new Vector3();

    private void Start()
    {
        items = new List<Transform>();
        var size = gameObject.transform.childCount;
        for (var i = 0; i < size; i++)
        {
            var child = gameObject.transform.GetChild(i);
            items.Add(child);
        }
        validateState();
    }

    private void validateState()
    {
        if (mState)
        {
            var offset = gameObject.GetComponent<RectTransform>().rect.width + spacing;
            var size = items.Count;
            for (var i = 0; i < size; i++)
            {
                var child = items[i];
                child.gameObject.SetActive(true);
                CanvasGroup canvas = child.gameObject.GetComponent<CanvasGroup>();
                if (canvas != null)
                {
                    canvas.alpha = 1;
                }
                Debug.Log("" + offset );
                tmp.Set(child.localPosition.x, child.localPosition.y, child.localPosition.z);
                tmp.x = offset ;
                child.localPosition = tmp;
                Debug.Log("" +tmp.x);
                offset +=  child.GetComponent<RectTransform>().rect.width + spacing;
            }
        }
        else
        {
            var size = items.Count;
            for (var i = 0; i < size; i++)
            {
                var child = gameObject.transform.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }
    }

    public void toggle()
    {
        show(!state);
    }

    public void show(bool animate)
    {
        if (animate)
        {
            mState = true;
            var offset = gameObject.GetComponent<RectTransform>().rect.width  + spacing;
            var size = items.Count;
            for (var i = 0; i < size; i++)
            {
                var child = items[i];
                child.gameObject.SetActive(true);
                child.DOLocalMoveX(offset , duration);

                CanvasGroup canvas = child.gameObject.GetComponent<CanvasGroup>();
                if (canvas != null)
                {
                    canvas.alpha = 0;
                    canvas.DOFade(1, duration);
                }

                offset += child.GetComponent<RectTransform>().rect.width + spacing;
            }
        }
        else
        {
            mState = false;
            var size = items.Count;
            for (var i = 0; i < size; i++)
            {
                var child = items[i];
                child.gameObject.SetActive(true);
                var c = child.DOLocalMoveX(0, duration);
                c.OnComplete(()=> { child.gameObject.SetActive(false); });

                CanvasGroup canvas = child.gameObject.GetComponent<CanvasGroup>();
                if (canvas != null)
                {
                    canvas.DOFade(0, duration);
                }

            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
