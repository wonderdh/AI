using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject SettingPanel;

    [SerializeField]
    IngameManager igManager;

    private void Start()
    {
        SettingPanel.SetActive(false);
    }

    public void OpenMenuButton()
    {
        SettingPanel.SetActive(true);
    }

    public void ExitMenuButton()
    {
        // 저장하고 나가기
        SettingPanel.SetActive(false);
    }

    public void RestartButton()
    {
        // 저장 안하고 다시시작 or 초기화 기능
        igManager.resetMapInfo();

        SceneController.Instance.setTargetScene(SceneController.Instance.GetActiveScene().name);
    }

    public void HomeButton()
    {
        igManager.saveMapInfo();
        SceneController.Instance.setTargetScene("MapSelectSwipe");
    }
}
