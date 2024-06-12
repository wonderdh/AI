using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectSwipe : MonoBehaviour
{
    public Scrollbar scrollbar;

    float scroll_pos = 0;
    float[] pos;

    [SerializeField]
    Image panel;

    Color[] colors = { new Color(141/255f, 253 / 255f, 255 / 255f), new Color(206 / 255f, 255 / 255f, 219 / 255f), new Color(243 / 255f, 191 / 255f, 242 / 255f)};

    [SerializeField]
    TMP_Text mapNameText;
    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TMP_Text progress;

    MapSelectInfos msif;

    string focusedMapName;

    void Start()
    {
        msif = BuhitDB.Instance.getMapSelectInfos();

        //msif.printThis();

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
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (1f / (pos.Length * 2f)) && scroll_pos > pos[i] - (1f / (pos.Length * 2f)))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], 0.1f);
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
            panel.color = colors[index];

            mapNameText.text = msif.Name[index];

            /*if (Mathf.Abs(progressBar.value - msif.Progress[index]) > 0.00001f)
            {
                StartCoroutine(LerpProgressBar(msif.Progress[index]));
            }*/
            progressBar.value = msif.Progress[index];
            progress.text = msif.Progress[index] + "%";

            focusedMapName = msif.Name[index];
        }
    }

    private IEnumerator LerpProgressBar(float progressValue)
    {
        float delta = 0f;
        float duration = 0.2f;
        float startValue = 0;

        while (delta <= duration)
        {
            float t = delta / duration;
            progressBar.value = Mathf.Lerp(startValue, progressValue, t);

            delta += Time.deltaTime;
            yield return null;
        }
    }

    public void ChangeScene()
    {
        SceneController.Instance.setTargetScene(focusedMapName);
    }
}
