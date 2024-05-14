using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Check_list : MonoBehaviour
{

    public void ClickBtn()
    {
        print("버튼 클릭");

        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;

        print(clickedObject.name);

        GameObject clImg = GameObject.Find("cl" + clickedObject.name);

        Image img = clImg.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        GameObject ck = clImg.transform.GetChild(0).gameObject;
        ck.SetActive(true);

    }
}