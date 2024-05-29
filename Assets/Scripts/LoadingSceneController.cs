using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Setting_loadingcontroll : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSettingSceneWithDelay());
    }

    private IEnumerator LoadSettingSceneWithDelay()
    {
        // 1초 대기
        yield return new WaitForSeconds(1);

        // 설정 씬으로 전환 (SettingScene으로 가정)
        SceneManager.LoadScene("setting");
    }
}
