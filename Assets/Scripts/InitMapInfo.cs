using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;
using TMPro;

public class InitMapInfo : MonoBehaviour
{
    string[] mapNames = { "Haeundae"};

    [SerializeField]
    TMP_Text message;

    [SerializeField]
    JSonController jc;

    void Start()
    {
        string path = Application.persistentDataPath + "/MapInfo";
        message.text = path;

        if (!System.IO.File.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        for(int i = 0; i < mapNames.Length; i++)
        {
            if (!System.IO.File.Exists(path + "/" + mapNames[i] + ".json"))
            {
                Debug.Log("no");
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                dicList = CSVReader.Read(mapNames[i]);

                string[] checkedList = new string[dicList.Count];
                string[] descriptionList = new string[dicList.Count];

                for(int c = 0; c < dicList.Count; c++)
                {
                    checkedList[c] = dicList[c]["checkedList"].ToString();
                    descriptionList[c] = dicList[c]["descriptionList"].ToString();
                }

                MapInfo mapInfo = new MapInfo(checkedList, descriptionList);

                string jsonInfo = JsonUtility.ToJson(mapInfo);

                path += "/" + mapNames[i] + ".json";

                System.IO.File.WriteAllText(path, jsonInfo);
            }
        }
    }
}
