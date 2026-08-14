using UnityEngine;
using UnityEngine.EventSystems;

public class MobileAirPump : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] RectTransform handle;
    [SerializeField] NewMovementScript move;
    [SerializeField] SoundScript sound;
    [SerializeField] Vector2 topPoint;
    [SerializeField] Vector2 bottomPoint;
    [SerializeField] float threshold = 5f;
    [SerializeField] int count = 0;

    private float t;

    public void OnBeginDrag(PointerEventData eventData)
    {
        t = GetProjectionT(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        t = GetProjectionT(eventData);
        t = Mathf.Clamp01(t);
        handle.anchoredPosition = Vector2.Lerp(bottomPoint, topPoint, t);
        Press();
    }

    float GetProjectionT(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handle.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        Vector2 lineDir = topPoint - bottomPoint;
        Vector2 fromBottom = localPoint - bottomPoint;

        float length = lineDir.magnitude;

        if (length == 0) return 0;

        Vector2 dir = lineDir / length;
        float projection = Vector2.Dot(fromBottom, dir);

        return projection / length;
    }

    void Press()
    {
        float distanceToTop = Vector2.Distance(handle.anchoredPosition, topPoint);
        float distanceToBottom = Vector2.Distance(handle.anchoredPosition, bottomPoint);

        if (distanceToTop <= threshold && count == 0)
        {
            count = 1;
            move.Push();
            sound.Play(3);
        }

        if (distanceToBottom <= threshold && count == 1)
        {
            count = 0;
            move.Push();
            sound.Play(4);
        }
    }

}