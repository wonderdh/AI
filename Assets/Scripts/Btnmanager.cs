using UnityEngine;
using UnityEngine.UI;

public class Btnmanager : MonoBehaviour
{ 
    public Sprite[] Sprites;
    public bool isClicked;

    public void BtnClick()
    {
        GameObject.Find("Image").GetComponent<Image> ().sprite = Sprites[0];

    }
}
