using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

static internal class UIButtonEditor
{
    [MenuItem("GameObject/UI/UI Scale Button", false)]
    public static void AddUIButton(MenuCommand menuCommand)
    {
        GameObject parent = menuCommand.context as GameObject;

        GameObject go = new GameObject("Button");
        go.AddComponent<RectTransform>();
        GameObjectUtility.SetParentAndAlign(go, parent);

        Image image = go.AddComponent<Image>();
        image.type = Image.Type.Sliced;

        ESUIButton bt = go.AddComponent<ESUIButton>();
    }
}
#endif

public class ESUIButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public static float ButtonClickEffectTime = 0.1f;
    public static float ButtonClickMinScale = 0.95f;
    public static float ButtonClickMaxScale = 1.05f;
    public UnityEvent _onClick;
    public bool isScaleWhenClicked = true;

    private Vector3 normalScale = Vector3.one;
    private bool isPlayingEffect;

    private bool isEnable = true;
    private GrayScale grayscale;

    //private Vector3 pointerClickPos;

    public virtual void Awake()
    {
        normalScale = transform.localScale;
        if (grayscale == null) grayscale = GetComponent<GrayScale>();
    }

    public virtual void OnPointerDown(PointerEventData e)
    {
        if (!isEnable) return;
        if (isScaleWhenClicked)
        {
            transform.DOScale(normalScale * ButtonClickMinScale, ButtonClickEffectTime).SetUpdate(true);
        }
    }

    public virtual void OnPointerUp(PointerEventData e)
    {
        
            if (!isEnable) return;
        if (isScaleWhenClicked)
        {
            DOTween.Complete(transform);
            transform.DOScale(normalScale, ButtonClickEffectTime).SetUpdate(true);
        }
    }

    public virtual void OnPointerClick(PointerEventData e)
    {
        if (!isEnable) return;
        if (isPlayingEffect) return;
        if (isScaleWhenClicked)
        {
            DOTween.Complete(transform);
            transform.localScale = normalScale * ButtonClickMinScale;
            CoReleaseAnimation();
        }
        else CallClickedAction();
    }

    private void CoReleaseAnimation()
    {
        isPlayingEffect = true;
        var sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(normalScale * ButtonClickMaxScale, ButtonClickEffectTime));
        sequence.Append(transform.DOScale(normalScale, ButtonClickEffectTime));

        sequence.SetUpdate(true);

        sequence.OnComplete(() =>
        {
            CallClickedAction();
        });
    }

    public virtual void CallClickedAction()
    {
        isPlayingEffect = false;
        _onClick.Invoke();
    }

    private void OnEnable()
    {
        DOTween.Complete(transform);
        if (isScaleWhenClicked) transform.localScale = normalScale;
        isPlayingEffect = false;
    }

    private void OnDisable()
    {
        DOTween.Complete(transform);
        if (isScaleWhenClicked) transform.localScale = normalScale;
        isPlayingEffect = false;
    }

    public void SetEnableUIButton(bool enable)
    {
        if (enable != isEnable)
        {
            isEnable = enable;
            if (grayscale == null) grayscale = GetComponent<GrayScale>();
            if (grayscale != null) grayscale.SetGrayScale(!isEnable);
        }
    }
}
