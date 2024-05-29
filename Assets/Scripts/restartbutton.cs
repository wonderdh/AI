using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class restartbutton : MonoBehaviour
{
    public void SceneChange()
    {
        StartCoroutine(LoadScene());
        Debug.Log("Clicked");
    }

    private IEnumerator LoadScene()
    {
        // 로딩 씬으로 전환
        SceneManager.LoadScene("GamePlay");

    
        yield return null;
    }
}

