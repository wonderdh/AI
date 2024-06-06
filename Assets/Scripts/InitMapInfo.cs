using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using TMPro;

public class InitMapInfo : MonoBehaviour
{
    string[] mapNames = { "Haeundae", "Baseball" };
    int[] mapObjects = { 13, 13 };

    [SerializeField]
    TMP_Text message;

    string srcFile = Application.dataPath + "/Resources/MapInfo/";

    [SerializeField]
    JSonController jc;

    void Start()
    {
        string path = Application.persistentDataPath + "/MapInfo/";

        if (!System.IO.File.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        for (int i = 0; i < mapNames.Length; i++)
        {
            if (!System.IO.File.Exists(path + "/" + mapNames[i] + ".json"))
            {
                MapInfo mapInfo = new MapInfo(mapObjects[i]);

                string ju = JsonUtility.ToJson(mapInfo);
                System.IO.File.WriteAllText(path + mapNames[i] + ".json", ju);

                message.text += "new";
                message.text += mapNames[i] + " ";
            } 
        }
    }
}
