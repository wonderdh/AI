using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    string mapName;

    [SerializeField]
    GameObject Background;

    [SerializeField]
    string mapPath;

    List<Dictionary<string, object>> data_Dialog;

    void Start()
    {
        mapName = "01.Haeundae";
        Background = GameObject.Find("Background");
        mapPath = Application.dataPath + "/Maps/" + mapName;

        Debug.Log(mapPath);

        data_Dialog = CSVReader.Read(mapName);

        for (int i = 0; i < data_Dialog.Count; i++)
        {
            Debug.Log(data_Dialog[i]["id"].ToString() + data_Dialog[i]["name"].ToString() + data_Dialog[i]["X"].ToString() + data_Dialog[i]["Y"].ToString());   
            
            //���⼭ �� ���� �̹��� ���� �� ��������. �� ����ŭ ������Ʈ ����� �Ҳ���.
        }
    }
}
