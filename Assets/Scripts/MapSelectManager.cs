using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapSelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_Text star;


    void Start()
    {
        star.text = "x " + BuhitDB.Instance.GetStar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
