using UnityEngine;
using UnityEngine.UI;

public class DragCam : MonoBehaviour
{
    [SerializeField] GameObject sr;

    public void Awake()
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< Updated upstream
        sr = GameObject.Find("BgContent");
=======
        sr = GameObject.Find("Scroll View");
>>>>>>> Stashed changes
=======
        sr = GameObject.Find("Content");
>>>>>>> parent of 6146496 (Merge pull request #5 from wonderdh/main)
=======
        sr = GameObject.Find("Content");
>>>>>>> parent of 6146496 (Merge pull request #5 from wonderdh/main)
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
