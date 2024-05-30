using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject SettingPanel;

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
        SettingPanel.SetActive(false);
    }

    public void RestartButton()
    {
        SceneController.Instance.SceneChange(SceneController.Instance.GetActiveScene().name);
    }

}
