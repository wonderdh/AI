using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ScrollTestScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    RectTransform scrollRect;

    [SerializeField]
    float scrollSpeed = 50f;

    [SerializeField]
    GameObject collider;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.delta.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.delta.y);

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        float rectMoveY = eventData.scrollDelta.y * Time.deltaTime * scrollSpeed;

        scrollRect.anchoredPosition = new Vector2(scrollRect.anchoredPosition.x, rectMoveY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }



    /*
    public void ScrollList()
    {
        if (Input.touchCount == 1)
        {

            if (!IsPointerOverUIObject())
            {
                return;
            }

            Touch touch = Input.touches[0];

            float rectMoveY = scrollRect.anchoredPosition.y + (touch.deltaPosition.y * Time.deltaTime * scrollSpeed);

            scrollRect.anchoredPosition = new Vector2(scrollRect.anchoredPosition.x, rectMoveY);
        }

        CollisionCheck cl = collider.GetComponent<CollisionCheck>();

        GameObject cO = cl.GetLatestCollsionObject();

        Debug.Log(cO.transform.name);
    }

    public void Update()
    {
        ScrollList();
    }
    */




}