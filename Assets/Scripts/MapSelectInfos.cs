using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MapSelectInfos 
{
    public int[] ID;
    public string[] Name;
    public float[] Progress;
    public int[] Stars;
    public int[] isCleared;
    public int[] isUnlocked;

    public MapSelectInfos(int[] ID, string[] Name, float[] Progress, int[] Stars, int[] isCleared, int[] isUnlocked)
    {
        this.ID = ID;
        this.Name = Name;
        this.Progress = Progress;  
        this.Stars = Stars;
        this.isCleared = isCleared;
        this.isUnlocked = isUnlocked;
    }


    public void printThis() {
        for(int i = 0; i < ID.Length; i++)
        {
            Debug.Log(ID[i]);
            Debug.Log(Name[i]);
            Debug.Log("------------");
        }
    }
}
