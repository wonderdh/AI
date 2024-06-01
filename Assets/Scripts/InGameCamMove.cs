using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InGameCamMove : MonoBehaviour
{
    [SerializeField]
    float zoomIn = 2f;
    float zoomOut = 3.8f;

    [SerializeField]
    float maxCamMoveX = 17f;
    [SerializeField]
    float maxCamMoveY = 13f;

    bool isMoving = true;
    bool isMoveable = true;

    enum GESTURE
    {
        MOVE = 1,
        ZOOM,
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamTouch();
        ZoomCamTouch();
        moveCheck();
    }

    private void moveCheck()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isMoveable = true;
            isMoving = false;
        }
    }

    private void MoveCamTouch()
    {
        if (isMoveable == false)
        {
            return;
        }

        if (Input.touchCount == (int)GESTURE.MOVE)
        {
            if (SceneController.Instance.IsPointerOverUIObject() && !isMoving)
            {
                isMoveable = false;
                return;
            }

            isMoving = true;

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

    private void ZoomCamTouch()
    {
        if (Input.touchCount == (int)GESTURE.ZOOM)
        {
            if (SceneController.Instance.IsPointerOverUIObject())
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
