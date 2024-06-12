using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public string[] id;
    public int[] checkedList;

    public MapInfo(int n)
    {
        this.id = new string[n];
        this.checkedList = new int[n];

        for(int i = 0; i < n; i++)
        {
            id[i] = i.ToString();
            checkedList[i] = 0;
        }
    }
}
