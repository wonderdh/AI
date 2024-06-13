using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MapSelectSwipe : MonoBehaviour
{
    public Scrollbar scrollbar;

    float scroll_pos = 0;
    float[] pos;

    [SerializeField]
    Image panel;

    [SerializeField]
    TMP_Text star;

    Color[] colors = { new Color(141/255f, 253 / 255f, 255 / 255f), new Color(206 / 255f, 255 / 255f, 219 / 255f), new Color(243 / 255f, 191 / 255f, 242 / 255f)};

    [SerializeField]
    TMP_Text mapNameText;
    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TMP_Text progress;

    [SerializeField]
    TMP_Text buttonTxt;

    MapSelectInfos msif;
    int currentStar;

    int focusedMapIndex;

    [SerializeField]
    GameObject unlockPanel;
    [SerializeField]
    Image unlockThumbnail;
    [SerializeField]
    TMP_Text unlockPanelMapName;
    Sprite[] mapImages;
    [SerializeField]
    GameObject unlockButton;

    [SerializeField]
    GameObject messagePanel;
    [SerializeField]
    GameObject SuccessMessage;
    [SerializeField]
    GameObject FailMessage;

    void Start()
    {
        msif = BuhitDB.Instance.getMapSelectInfos();

        currentStar = BuhitDB.Instance.GetStar();

        star.text = "x " + currentStar;

        unlockPanel.SetActive(false);
        messagePanel.SetActive(false);

        //msif.printThis();
        mapImages = new Sprite[transform.childCount];

        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i; // 로컬 변수를 사용하여 클로저 문제를 해결합니다.
            Transform childT = transform.GetChild(i);

            childT.GetComponent<Button>().onClick.AddListener(() => OnElementClick(index));
            
            Image childImg = childT.GetComponent<Image>();
            mapImages[index] = childImg.sprite;

            if (msif.isUnlocked[i] == 1) // 해금 됐으면 1 아니면 0
            {
                childImg.color = new Color(childImg.color.r, childImg.color.g, childImg.color.b, 1f);
                childT.GetChild(0).gameObject.SetActive(false);
                childT.GetChild(1).gameObject.SetActive(false);
            }
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

            progressBar.value = msif.Progress[index];
            progress.text = (msif.Progress[index] * 100) + "%";

            focusedMapIndex = index;

            if (msif.isUnlocked[index] == 1)
            {
                buttonTxt.text = "Play!";
            } else
            {
                buttonTxt.text = "Unlock";
            }
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

    public void OnClickRedButton()
    {
        if (msif.isUnlocked[focusedMapIndex] == 1)
        {
            SceneController.Instance.setTargetScene(msif.Name[focusedMapIndex]);
        } else
        {
            unlockThumbnail.sprite = mapImages[focusedMapIndex];
            unlockPanelMapName.text = msif.Name[focusedMapIndex];

            unlockButton.SetActive(true);
            unlockPanel.SetActive(true);

            Debug.Log("사라");
        }
    }

    public void closeUnlockPanel()
    {
        messagePanel.SetActive(false);
        unlockPanel.SetActive(false);

        SceneController.Instance.ReloadScene();
    }

    public void onClickUnlockButton()
    {
        unlockButton.SetActive(false);

        if (currentStar >= 3)
        {
            currentStar -= 3;

            BuhitDB.Instance.onUnlock(currentStar, focusedMapIndex);
            
            Debug.Log("unlock success!!" + focusedMapIndex);

            FailMessage.SetActive(false);
            SuccessMessage.SetActive(true);

            messagePanel.SetActive(true);
        } else
        {
            FailMessage.SetActive(true);
            SuccessMessage.SetActive(false);

            messagePanel.SetActive(true);
            Debug.Log("unlock failed...");
        }
    }
}
