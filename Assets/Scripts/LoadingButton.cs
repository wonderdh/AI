using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingButton : MonoBehaviour
{   
    public void startGame()
    {
        SceneManager.LoadScene("subway_door");
    }

}
