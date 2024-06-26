using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    // Map
    [SerializeField]
    GameObject background;

    [SerializeField]
    private GameObject[] hiddenObject;

    [SerializeField]
    GameObject content;
    [SerializeField]
    int contentCount;

    [SerializeField]
    GameObject[] hiddenObjectList;

    GameObject touchedObject = null;

    MapInfo mapInfo = null;

    //Anim
    [SerializeField]
    GameObject findAnimPrefab;

    //ProgressBar
    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TMP_Text progressText;

    //Star
    [SerializeField]
    GameObject[] starList;

    int totalObjects = 0;
    int checkedObject = 0;

    int[] starGive = new int[3];

    public int getStar = 0;

    // Audio
    [SerializeField]
    AudioSource buttonAudioSource;
    [SerializeField]
    AudioClip[] audioClip;

    // Swipe
    public Scrollbar scrollbar;
    public GameObject descriptionImg;
    public TMP_Text descriptionText;

    public string[] description;

    float scroll_pos = 0;
    float[] pos;

    public void Start()
    {
        initBackground();

        initMapInfo();
        initContent();

        SetProgressBar();

        for (int i = 0; i < starGive.Length; i++)
        {
            starGive[i] = (i + 1) * (totalObjects / 3);
        }

        starGive[2] = totalObjects;

        checkStar();

        initSwipe();
    }

    public void Update()
    {
        swipeUpdate();
        CheckTouch();
    }

    public void CheckTouch()
    {
        OnTouchStart();
        OnTouchEnd();
    }

    // 터치 감지
    private void OnTouchStart()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            touchedObject = null;

            if (SceneController.Instance.IsPointerOverUIObject())
            {
                return;
            }

            descriptionImg.SetActive(false);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!(hit.transform.tag == "HiddenObjects"))
                {
                    return;
                }

                touchedObject = hit.transform.gameObject;
            }
        }
    }

    private void OnTouchEnd()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (SceneController.Instance.IsPointerOverUIObject())
            {
                return;
            }

            descriptionImg.SetActive(false);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == touchedObject)
                {
                    for (int i = 0; i < hiddenObject.Length; i++)
                    {
                        if (hiddenObject[i] == hit.collider.gameObject)
                        {
                            // 이미 찾은건지 아닌지 확인
                            if (mapInfo.checkedList[i] == 1)
                            {
                                break;
                            }

                            // 이미 찾은게 아닐 경우
                            CheckMark(hiddenObjectList[i]);

                            GameObject findAnim = Instantiate(findAnimPrefab, hiddenObject[i].transform);

                            mapInfo.checkedList[i] = 1; // 업데이트

                            checkedObject++;
                            SetProgressBar();
                            checkStar();

                            buttonAudioSource.Stop();
                            buttonAudioSource.PlayOneShot(audioClip[1]);

                            MoveScrollToCheckedObject(i);

                            break;
                        }
                    }
                }
            }
        }
    }


    // 배경 및 리스트 초기화
    private void initBackground()
    {
        if (background == null)
        {
            background = GameObject.FindGameObjectWithTag("Background");

            contentCount = background.transform.childCount;

            hiddenObject = new GameObject[contentCount];

            for (int i = 0; i < contentCount; i++)
            {
                hiddenObject[i] = background.transform.GetChild(i).gameObject;
            }
        }
        else
        {
            contentCount = background.transform.childCount;
        }

        mapInfo = new MapInfo(contentCount);

        description = BuhitDB.Instance.DB_Description(contentCount);
    }

    private void initContent()
    {
        if (content == null)
        {
            content = GameObject.FindGameObjectWithTag("Content");

            int childCount = content.transform.childCount;

            hiddenObjectList = new GameObject[childCount];

            totalObjects = childCount;

            for (int i = 0; i < childCount; i++)
            {
                hiddenObjectList[i] = content.transform.GetChild(i).gameObject;

                if (mapInfo.checkedList[i] == 1)
                {
                    CheckMark(hiddenObjectList[i]);
                    checkedObject++;
                }
            }
        }
    }

    private void SetProgressBar()
    {
        StartCoroutine(LerpProgressBar());
    }

    private IEnumerator LerpProgressBar()
    {
        float delta = 0f;
        float duration = 0.5f;
        float startValue = progressBar.value;
        float endValue = (float)checkedObject / (float)totalObjects;

        while (delta <= duration)
        {
            float t = delta / duration;
            progressBar.value = Mathf.Lerp(startValue, endValue, t);

            //Debug.Log(progressBar.value);

            delta += Time.deltaTime;
            yield return null;
        }

        progressText.text = checkedObject.ToString() + " / " + totalObjects.ToString();

        if (progressBar.value >= 0.95f)
        {
            progressText.text = "Complete!!";

            buttonAudioSource.Stop();
            buttonAudioSource.PlayOneShot(audioClip[2]);
        }
    }

    private void CheckMark(GameObject go)
    {
        Image img = go.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        GameObject checkMark = go.transform.GetChild(0).gameObject;
        checkMark.SetActive(true);
    }
    private void MoveScrollToCheckedObject(int index)
    {
        float distance = 1f / (pos.Length - 1f);
        scroll_pos = pos[index];
        scrollbar.value = 1 - pos[index];
        UpdateDescription(index);
    }

    // 저장 및 맵 관련 기능들

    private void initMapInfo()
    {
        //mapInfo = JsonUtility.FromJson<MapInfo>(jsonC.LoadMapInfo(SceneController.Instance.GetActiveScene().name));
        mapInfo.checkedList = BuhitDB.Instance.DBisChecked(contentCount);
    }

    public void saveMapInfo()
    {
        float progress = (float)checkedObject / (float)totalObjects;

        BuhitDB.Instance.UpdateDb(mapInfo.checkedList, progress, getStar);
    }

    public void resetMapInfo()
    {
        BuhitDB.Instance.ResetMap(contentCount);
    }

    private void checkStar()
    {
        getStar = 0;

        for (int i = 0; i < starGive.Length; i++)
        {
            if (checkedObject >= starGive[i])
            {
                starList[i].gameObject.SetActive(true);
                getStar++;
                Debug.Log(checkedObject);
            }
            else
            {
                starList[i].gameObject.SetActive(false);
            }
        }
    }

    public void buttonClickSoundPlay()
    {
        buttonAudioSource.Stop();
        buttonAudioSource.PlayOneShot(audioClip[0]);
    }


    // contentList 스크롤 관련
    private void initSwipe()
    {
        pos = new float[content.transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        for (int i = 0; i < content.transform.childCount; i++)
        {
            int index = i; // 로컬 변수를 사용하여 클로저 문제를 해결합니다.
            content.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => OnElementClick(index));
        }
    }

    // swipe update 부분 수정 해야 할듯
    //
    private void swipeUpdate()
    {
        if (!SceneController.Instance.IsPointerOverUIObject())
        {
            return;
        }

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
                content.transform.GetChild(i).localScale = Vector2.Lerp(content.transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                UpdateDescription(i);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        content.transform.GetChild(a).localScale = Vector2.Lerp(content.transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
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
        if (index >= 0 && index < content.transform.childCount)
        {
            descriptionImg.SetActive(true);
            descriptionText.text = description[index];
        }
    }
}