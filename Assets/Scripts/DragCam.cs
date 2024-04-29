using UnityEngine;
using UnityEngine.UI;

public class DragCam : MonoBehaviour
{
    [SerializeField] GameObject sr;

    public void Awake()
    {
        sr = GameObject.Find("Content");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            float lsX = sr.transform.localScale.x;
            float lsY = sr.transform.localScale.y;
            
            if (lsX > 0.1 && lsY > 0.1)
            {
                sr.transform.localScale = new Vector3(lsX - (float)0.1, lsY - (float)0.1, sr.transform.localScale.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            float lsX = sr.transform.localScale.x;
            float lsY = sr.transform.localScale.y;

            if (lsX < 1 && lsY < 1)
            {
                sr.transform.localScale = new Vector3(lsX + (float)0.1, lsY + (float)0.1, sr.transform.localScale.z);
            }
        }
    }

}
