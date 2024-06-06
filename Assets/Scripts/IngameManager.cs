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
    GameObject[] hiddenObjectList;

    [SerializeField]
    GameObject findAnimPrefab;

    GameObject touchedObject = null;
    
    [SerializeField]
    JSonController jsonC;
    MapInfo mapInfo = null;

    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TMP_Text progressText;

    [SerializeField]
    GameObject[] starList;
    

    int totalObjects = 0;
    int checkedObject = 0;

    int[] starGive = new int[3];

    public int getStar = 0;

    public void Start()
    {
        initMapInfo();

        initBackground();
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
                            if (mapInfo.checkedList[i] == "o")
                            {
                                break;
                            }

                            // 이미 찾은게 아닐 경우
                            CheckMark(hiddenObjectList[i]);

                            GameObject findAnim = Instantiate(findAnimPrefab, hiddenObject[i].transform);

                            mapInfo.checkedList[i] = "o"; // 업데이트

                            checkedObject++;
                            SetProgressBar();
                            checkStar();

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

            int childCount = background.transform.childCount;

            hiddenObject = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                hiddenObject[i] = background.transform.GetChild(i).gameObject;
            }
        }
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

                if (mapInfo.checkedList[i] == "o")
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
        mapInfo = JsonUtility.FromJson<MapInfo>(jsonC.LoadMapInfo(SceneController.Instance.GetActiveScene().name));
    }

    public void saveMapInfo()
    {
        jsonC.SaveMapInfo(SceneController.Instance.GetActiveScene().name, JsonUtility.ToJson(mapInfo));
    }

    public void resetMapInfo()
    {
        for(int i = 0; i < mapInfo.checkedList.Length; i++)
        {
            mapInfo.checkedList[i] = "x";
        }

        jsonC.SaveMapInfo(SceneController.Instance.GetActiveScene().name, JsonUtility.ToJson(mapInfo));
        // 여기에 추가
    }

    private void checkStar()
    {
        for(int i = 0; i < starGive.Length; i++)
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