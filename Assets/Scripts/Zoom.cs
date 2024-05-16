using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField]
    Transform Background;

    // Update is called once per frame
    void Update()
    {
        ZoomCam();
    }


    private void ZoomCam()
    {
        if (Input.touchCount == 2)
        {
            Touch touch_1 = Input.touches[0];
            Touch touch_2 = Input.touches[1];

            //���� �������� ��ġ ��ǥ�� ���Ѵ�.
            Vector2 t1PrevPos = touch_1.position - touch_1.deltaPosition;
            Vector2 t2PrevPos = touch_2.position - touch_2.deltaPosition;

            //���� �����Ӱ� ���� ������ ������ ũ�⸦ ����.
            float prevDeltaMag = (t1PrevPos - t2PrevPos).magnitude;
            float deltaMag = (touch_1.position - touch_2.position).magnitude;

            //�� ũ�Ⱚ�� ���� ���� �� ��/�ƿ��� ũ�Ⱚ�� ���Ѵ�.
            float deltaMagDiff = prevDeltaMag - deltaMag;

            Debug.Log(deltaMagDiff);

        }
    }
}
