using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public static SceneController Instance
    {
        get { 
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

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

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void MapSelcetScene()
    {
        SceneManager.LoadScene("MapSelect");
    }
}
