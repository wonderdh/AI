using System.Collections.Generic;
using UnityEngine;

public class CSVManager : MonoBehaviour
{
    [SerializeField]
    string mapName;

    string csvPath = "MapInfo/";

    string[] checkedList;
    public string[] descriptionList;

    List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    private void Awake()
    {
        mapName = SceneController.Instance.GetActiveScene().name;
        csvPath += mapName;
        dicList = CSVReader.Read(csvPath);

        checkedList = new string[dicList.Count];
        descriptionList = new string[dicList.Count];
        
        for(int i = 0; i < checkedList.Length; i++)
        {
            checkedList[i] = dicList[i]["checked"].ToString();
            descriptionList[i] = dicList[i]["description"].ToString();
        }
    }

    public string GetDescriptions(int i)
    {
        return descriptionList[i];
    }

    public bool GetChecekd(int i)
    {
        if (checkedList[i] == "o")
        {
            return true;
        }

        return false;
    }

    public void UpdateCheck(int i)
    {
        checkedList[i] = "o";
    }
}


/*
public void GenerateMap()
{
                                   // Ÿ�� csv���� �ҷ�����
                                                                        // List Scroll ���� �ִ� Content�� �߰� �� ����Ʈ

    hoTr.localPosition = new Vector3(float.Parse(dicList[i]["posX"].ToString()), float.Parse(dicList[i]["posY"].ToString()), 0);
    hoTr.localScale = new Vector3(float.Parse(dicList[i]["scaleX"].ToString()), float.Parse(dicList[i]["scaleY"].ToString()), 0);

}
*/