using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public string[] id;
    public string[] checkedList;

    public MapInfo(int n)
    {
        this.id = new string[n];
        this.checkedList = new string[n];

        for(int i = 0; i < n; i++)
        {
            id[i] = i.ToString();
            checkedList[i] = "x";
        }
    }
}
