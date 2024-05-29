using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class homebutton : MonoBehaviour
{
    public void SceneChangeWithLoading()
    {
        StartCoroutine(LoadScenesWithDelay());
        Debug.Log("Clicked");
    }

    private IEnumerator LoadScenesWithDelay()
    {
        // 로딩 씬으로 전환
        SceneManager.LoadScene("UI_Menu");

    
        yield return null;
    }
}

