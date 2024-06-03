using System.IO;
using UnityEngine;

public class JSonController : MonoBehaviour
{
    public void SaveMapInfo(string mapName, string mapInfo)
    {
        string path = Application.persistentDataPath + "/MapInfo/" + mapName + ".json";

        File.WriteAllText(path, mapInfo);
    }

    public string LoadMapInfo(string mapName)
    {
        string path = Application.persistentDataPath + "/MapInfo/" + mapName + ".json";
        
        return File.ReadAllText(path);
    }
}
