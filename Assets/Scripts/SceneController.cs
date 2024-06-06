using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        Application.targetFrameRate = 60;
    }

    public static SceneController Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    [SerializeField]
    private string targetScene = "";

    public Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void setTargetScene(string sceneName)
    {
        ChangeTarget(sceneName);
        Debug.Log(targetScene);
        changeScene();
    }

    private void ChangeTarget(string sceneName)
    {
        this.targetScene = sceneName;
    }

    public void changeScene()
    {
        SceneManager.LoadScene(this.targetScene);
    }

    IEnumerator waiting()
    {
        float delay = 0;

        while(delay < 0.5f)
        {
            delay += Time.deltaTime;

            yield return null;
        }

        changeScene();
    }
}
