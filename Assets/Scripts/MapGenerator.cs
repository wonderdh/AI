using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;
using System.Text;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject Background;
    [SerializeField]
    GameObject[] HiddenObjects;

    [SerializeField]
    TMP_Text folderName;

    string filepath = Application.dataPath + "/Resources/MapInfo/";

    List<string[]> data = new List<string[]>();
    string[] tempData;

    public void Start()
    {
        HiddenObjects = new GameObject[Background.transform.childCount];

        for (int i = 0; i < Background.transform.childCount; i++)
        {
            HiddenObjects[i] = Background.transform.GetChild(i).gameObject;
        }
    }

    public void GenerateMap()
    {
        initList();

        for (int i = 0; i < HiddenObjects.Length; i++)
        {
            GameObject ho = HiddenObjects[i];

            string hoName = ho.name;
            Vector2 pos = new Vector2(ho.transform.position.x, ho.transform.position.y);
            Vector2 scale = new Vector2(ho.transform.localScale.x, ho.transform.localScale.y);

            tempData = new string[7];
            tempData[0] = hoName;
            tempData[1] = pos.x.ToString();
            tempData[2] = pos.y.ToString();
            tempData[3] = scale.x.ToString();
            tempData[4] = scale.y.ToString();
            tempData[5] = "";
            tempData[6] = "";
            data.Add(tempData);

            //Debug.LogFormat("name : {0}, posX : {1}, posY : {2}, scaleX : {3}, scaleY : {4}\n", hoName, pos.x, pos.y, scale.x, scale.y);
        }

        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        StreamWriter outStream = System.IO.File.CreateText(filepath + folderName.text + ".csv");
        outStream.Write(sb);
        outStream.Close();

        Debug.Log("File Generated!");
        Debug.Log(filepath + folderName.text + ".csv");
    }

    private void initList()
    {
        data.Clear();

        tempData = new string[7];
        tempData[0] = "Name";
        tempData[1] = "posX";
        tempData[2] = "posY";
        tempData[3] = "scaleX";
        tempData[4] = "scaleY";
        tempData[5] = "discription";
        tempData[6] = "checked";
        data.Add(tempData);
    }
}
