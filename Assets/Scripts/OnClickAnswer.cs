using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickAnswer : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Debug.Log("touch!");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                GameObject clImg = GameObject.Find("cl" + hit.collider.gameObject.name);

                Image img = clImg.GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

                GameObject ck = clImg.transform.GetChild(0).gameObject;
                ck.SetActive(true);
            } 
        }
    }
}
