using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Text descriptionText;

    [SerializeField]
    private CSVManager csvManager;

    float scroll_pos = 0;
    float[] pos;

    void Start()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i; // 로컬 변수를 사용하여 클로저 문제를 해결합니다.
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => OnElementClick(index));
        }

        UpdateDescription(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_pos = 1 - scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (1f / (pos.Length * 2f)) && scroll_pos > pos[i] - (1f / (pos.Length * 2f)))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, 1 - pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (1f / (pos.Length * 2f)) && scroll_pos > pos[i] - (1f / (pos.Length * 2f)))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                UpdateDescription(i);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    void OnElementClick(int index)
    {
        scroll_pos = pos[index];
    }

    void UpdateDescription(int index)
    {
        if (index >= 0 && index < transform.childCount)
        {
            descriptionText.text = csvManager.GetDescriptions(index);
        }
    }
}
