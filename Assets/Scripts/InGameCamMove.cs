using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameCamMove : MonoBehaviour
{
    [SerializeField]
    float zoomIn = 2f;
    float zoomOut = 3.8f;

    float maxCamMoveX = 7f;
    float maxCamMoveY = 7f;

    enum GESTURE
    {
        MOVE = 1,
        ZOOM,
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // Update is called once per frame
    void Update()
    {

        MoveCam();
        ZoomCam();

    }

    private void MoveCam()
    {
        if (Input.touchCount == (int)GESTURE.MOVE)
        {

            if (IsPointerOverUIObject())
            {
                return;
            }

            Touch touch = Input.touches[0];

            float camX = Camera.main.transform.position.x;
            float camY = Camera.main.transform.position.y;
            float touchX = touch.deltaPosition.x;
            float touchY = touch.deltaPosition.y;

            float camMoveX = camX - (touchX * Time.deltaTime);
            float camMoveY = camY - (touchY * Time.deltaTime);

            if (camMoveX >= maxCamMoveX || camMoveX <= -maxCamMoveX)
            {
                camMoveX = camX;
            }

            if (camMoveY >= maxCamMoveY || camMoveY <= -maxCamMoveY)
            {
                camMoveY = camY;
            }

            Camera.main.transform.position = new Vector3(
                camMoveX,
                camMoveY,
                Camera.main.transform.position.z);
        }
    }

    private void ZoomCam()
    {
        if (Input.touchCount == (int)GESTURE.ZOOM)
        {

            if (IsPointerOverUIObject())
            {
                return;
            }

            Touch touch_1 = Input.touches[0];
            Touch touch_2 = Input.touches[1];

            //이전 프레임의 터치 좌표를 구한다.
            Vector2 t1PrevPos = touch_1.position - touch_1.deltaPosition;
            Vector2 t2PrevPos = touch_2.position - touch_2.deltaPosition;

            //이전 프레임과 현재 프레임 움직임 크기를 구함.
            float prevDeltaMag = (t1PrevPos - t2PrevPos).magnitude;
            float deltaMag = (touch_1.position - touch_2.position).magnitude;

            //두 크기값의 차를 구해 줌 인/아웃의 크기값을 구한다.
            float deltaMagDiff = prevDeltaMag - deltaMag;

            Camera.main.orthographicSize += (deltaMagDiff * Time.deltaTime);
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomIn, zoomOut);
        }
    }
}
