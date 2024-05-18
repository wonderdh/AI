using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HiddenObjects : MonoBehaviour
{

    private void Start()
    {
        List<string> tmp = ReadTxt("C:\\AI\\AI\\Assets\\Scripts\\tmp.txt").Split(',').ToList();

        Debug.Log(tmp[0] + tmp[1] + tmp[2]);

        GameObject obj = new GameObject("Bird!");
        obj.transform.position = new Vector3(float.Parse(tmp[1]), float.Parse(tmp[1]), 0);
    }

    string ReadTxt(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadToEnd();
            reader.Close();
        }

        else
            value = "파일이 없습니다.";

        return value;
    }
}
