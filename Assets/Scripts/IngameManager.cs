using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
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

    [SerializeField]
    GameObject findAnimPrefab;

    GameObject touchedObject = null;
    
    MapInfo mapInfo = null;

    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TMP_Text progressText;

    [SerializeField]
    GameObject[] starList;

    [SerializeField]
    AudioSource audioSource1;
    [SerializeField]
    AudioSource audioSource2;

    public string[] description;

    int totalObjects = 0;
    int checkedObject = 0;

    int[] starGive = new int[3];

    public int getStar = 0;

    

    public void Awake()
    {
        initBackground();

        initMapInfo();
        initContent();

        SetProgressBar();

        for(int i = 0; i < starGive.Length; i++)
        {
            starGive[i] = (i + 1) * (totalObjects / 3);
        }

        starGive[2] = totalObjects;

        checkStar();
    }

    public void Update()
    {
        CheckTouch();
    }

    public void CheckTouch()
    {
        OnTouchStart();
        OnTouchEnd();
    }

    private void OnTouchStart() {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            touchedObject = null;

            if (SceneController.Instance.IsPointerOverUIObject())
            {
                return;
            }

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
                            audioSource1.Play();
                            break;
                        }
                    }
                }
            }
        }
    }

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
        } else
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

        while(delta <= duration)
        {
            float t = delta / duration;
            progressBar.value = Mathf.Lerp(startValue, endValue, t);

            //Debug.Log(progressBar.value);

            delta += Time.deltaTime;
            yield return null;
        }

        progressText.text = checkedObject.ToString() + " / " + totalObjects.ToString();

        if(progressBar.value >= 0.95f)
        {
            progressText.text = "Complete!!";
            audioSource2.Play();
        }
    }

    private void CheckMark(GameObject go)
    {
        Image img = go.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        GameObject checkMark = go.transform.GetChild(0).gameObject;
        checkMark.SetActive(true);
    }

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
                Debug.Log(getStar);
            } else
            {
                starList[i].gameObject.SetActive(false);
            }
        }
    }


}