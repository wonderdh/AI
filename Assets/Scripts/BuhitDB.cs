using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEditor;

public class BuhitDB : MonoBehaviour
{
    private static BuhitDB instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            StartCoroutine(DBCreate());
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }


    public static BuhitDB Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    string DBName = "/BuhitDB.db";

    IEnumerator DBCreate()
    {
        string filepath = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Platform == Android");
            filepath = Application.persistentDataPath + DBName;

            if (!File.Exists(filepath))
            {
                UnityWebRequest uwr = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets" + DBName);
                uwr.downloadedBytes.ToString();
                yield return uwr.SendWebRequest().isDone;
                File.WriteAllBytes(filepath, uwr.downloadHandler.data);
            }
        }
        else
        {
            Debug.Log("Platform == Pc");

            filepath = Application.dataPath + DBName;

            if (!File.Exists(filepath))
            {
                File.Copy(Application.streamingAssetsPath + DBName, filepath);
                Debug.Log("DB복사 완료");
            }
        }
    }

    private void Start()
    {
        //DBConnectionCheck();

        //DataBaseRead("Select * From Maps");
    }

    private void DataBaseRead(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public int[] DBisChecked(int contentCount)
    {
        string query = "Select isChecked From " + SceneController.Instance.GetActiveScene().name;

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();

        int[] isChecked = new int[contentCount];
        int i = 0;

        while (dataReader.Read())
        {
            //Debug.Log(dataReader.GetInt32(0));
            isChecked[i] = dataReader.GetInt32(0);
            i++;
        }

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        return isChecked;
    }

    public string[] DB_Description(int contentCount)
    {
        string query = "Select Description From " + SceneController.Instance.GetActiveScene().name;

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();

        string[] description = new string[contentCount];

        int i = 0;

        while (dataReader.Read())
        {
            description[i] = dataReader.GetString(0);
            i++;
        }

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        return description;
    }

    public void UpdateDb(int[] isChecked, float progress, int getStar)
    {
        string query;

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        // 전체 맵 정보 업데이트

        query = "SELECT * FROM Maps WHERE Name=\"" + SceneController.Instance.GetActiveScene().name + "\"";
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();
        dataReader.Read();

        int stars = (int)dataReader.GetInt64(3);
        int isCleared = (int)dataReader.GetInt64(4);

        if (isCleared < 1)
        {
            if (getStar >= 3)
            {
                isCleared = 1;
            }
        }

        int updateTotalStars = 0;

        Debug.Log("새로 반환된 star = " + getStar + ", 기존 Star = " + stars);

        if (getStar <= stars)
        {
            getStar = stars;
        }
        else
        {
            updateTotalStars = getStar - stars;
        }

        dataReader.Dispose();


        query = string.Format("UPDATE Maps SET Progress={0}, Stars={1}, isCleared={2} WHERE Name=\"{3}\"", progress, getStar, isCleared, SceneController.Instance.GetActiveScene().name);
        dbCommand.CommandText = query;
        dataReader = dbCommand.ExecuteReader();
        dataReader.Dispose();

        // 해당 맵의 isChecked 업데이트
        for (int i = 0; i < isChecked.Length; i++)
        {
            query = string.Format("UPDATE {0} SET isChecked={1} WHERE ID={2}", SceneController.Instance.GetActiveScene().name, isChecked[i], (i + 1));
            dbCommand.CommandText = query;
            dataReader = dbCommand.ExecuteReader();
            dataReader.Dispose();
        }

        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        if (updateTotalStars > 0)
        {
            UpdateTotalStars(updateTotalStars);
        }
    }

    public void UpdateTotalStars(int updateTotalStars)
    {
        Debug.Log(updateTotalStars);
        string query;

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        // 전체 맵 정보 업데이트

        query = string.Format("Select * From Star");
        //query =  "SELECT * FROM Maps";
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();
        dataReader.Read();
        int currentStar = dataReader.GetInt32(0);
        dataReader.Dispose();

        query = string.Format("Update Star Set Star={0}", updateTotalStars + currentStar);
        //query =  "SELECT * FROM Maps";
        dbCommand.CommandText = query;
        dataReader = dbCommand.ExecuteReader();
        dataReader.Read();

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void ResetMap(int contentCount)
    {
        string query = "Select Description From " + SceneController.Instance.GetActiveScene().name;

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;

        for (int i = 0; i < contentCount; i++)
        {
            query = string.Format("UPDATE {0} SET isChecked={1} WHERE ID={2}", SceneController.Instance.GetActiveScene().name, 0, (i + 1));
            dbCommand.CommandText = query;
            IDataReader dataReader = dbCommand.ExecuteReader();
            dataReader.Dispose();
            dataReader = null;
        }

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        SceneController.Instance.ReloadScene();
    }

    private void DBConnectionCheck()
    {
        try
        {
            IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open();

            if (dbConnection.State == ConnectionState.Open)
            {
                Debug.Log("DB연결 성공");
            }
            else
            {
                Debug.Log("연결실패(에러)");
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public string GetDBFilePath()
    {
        string filePath = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = "URI=file:" + Application.persistentDataPath + DBName;
        }
        else
        {
            filePath = "URI=file:" + Application.dataPath + DBName;
        }



        return filePath;
    }

    public int GetStar()
    {
        string query = "Select Star From Star";

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();

        dataReader.Read();

        int star = (int)dataReader.GetInt64(0);

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        return star;
    }

    public MapSelectInfos getMapSelectInfos()
    {
        string query;

        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();

        query = string.Format("Select * From Maps");
        dbCommand.CommandText = query;
        IDataReader dataReader = dbCommand.ExecuteReader();

        int dataCount = 0;
        while (dataReader.Read())
        {
            dataCount++;
        }
        Debug.Log(dataCount);

        dataReader.Dispose();

        dataReader = dbCommand.ExecuteReader();

        int[] ID = new int[dataCount];
        string[] Name = new string[dataCount];
        float[] Progress = new float[dataCount];
        int[] Stars = new int[dataCount];
        int[] isCleared = new int[dataCount];
        int[] isUnlocked = new int[dataCount];

        int i = 0;
        while (dataReader.Read())
        {
            ID[i] = (int)dataReader.GetInt64(0); ;
            Name[i] = dataReader.GetString(1);
            Progress[i] = dataReader.GetFloat(2);
            Stars[i] = (int)dataReader.GetInt64(3);
            isCleared[i] = (int)dataReader.GetInt64(4);
            isUnlocked[i] = (int)dataReader.GetInt64(5);
            i++;
        }

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        return new MapSelectInfos(ID, Name, Progress, Stars, isCleared, isUnlocked);
    }

    public void resetDB()
    {
        StartCoroutine(DBReset());
    }

    IEnumerator DBReset()
    {
        string filepath = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Platform == Android");
            filepath = Application.persistentDataPath + DBName;

            if (File.Exists(filepath))
            {
                UnityWebRequest uwr = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets" + DBName);
                uwr.downloadedBytes.ToString();
                yield return uwr.SendWebRequest().isDone;
                File.Delete(filepath);
                File.WriteAllBytes(filepath, uwr.downloadHandler.data);
                Debug.Log("DB덮어쓰기 완료");
            }           
        }
        else
        {
            Debug.Log("Platform == Pc");

            filepath = Application.dataPath + DBName;

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
                File.Copy(Application.streamingAssetsPath + DBName, filepath);
                Debug.Log("DB덮어쓰기 완료");
            }
        }
    }
}