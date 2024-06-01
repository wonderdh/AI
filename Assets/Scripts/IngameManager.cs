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

    CSVManager csvM;

    GameObject touchedObject = null;

    public void Start()
    {
        csvM = transform.GetComponent<CSVManager>();

        initBackground();
        initContent();
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
                            if (csvM.GetChecekd(i))
                            {
                                break;
                            }

                            // 이미 찾은게 아닐 경우
                            CheckMark(hiddenObjectList[i]);

                            GameObject findAnim = Instantiate(findAnimPrefab, hiddenObject[i].transform);

                            csvM.UpdateCheck(i);
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

            for (int i = 0; i < childCount; i++)
            {
                hiddenObjectList[i] = content.transform.GetChild(i).gameObject;

                if (csvM.GetChecekd(i))
                {
                    CheckMark(hiddenObjectList[i]);
                }
            }
        }
    }

    private void CheckMark(GameObject go)
    {
        Image img = go.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);

        GameObject checkMark = go.transform.GetChild(0).gameObject;
        checkMark.SetActive(true);
    }

}