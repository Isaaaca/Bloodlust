using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager 
{
    private static HashSet<string> gameProgress = new HashSet<string>();
    private static XmlSerializableCounters counters = new XmlSerializableCounters();
    public static Vector2 playerSpawnPoint;
    public static int currentLevelSceneCode = 1;


    private static XmlSerializer hashSetSerializer = new XmlSerializer(typeof(HashSet<string>));
    private static XmlSerializer counterSerializer = new XmlSerializer(typeof(XmlSerializableCounters));



    public static void Save()
    {
        PlayerPrefs.DeleteAll();
        string xml;

        StringWriter stringWriter = new StringWriter();
        hashSetSerializer.Serialize(stringWriter, gameProgress);
        xml = stringWriter.ToString();
        PlayerPrefs.SetString("gameProgress", xml);

        stringWriter = new StringWriter();
        counterSerializer.Serialize(stringWriter, counters);
        xml = stringWriter.ToString();
        PlayerPrefs.SetString("counters", xml);

        PlayerPrefs.SetFloat("PlaerPosX", playerSpawnPoint.x);
        PlayerPrefs.SetFloat("PlaerPosY", playerSpawnPoint.y);
        PlayerPrefs.SetInt("CurrLevel", currentLevelSceneCode);

    }

    public static void Load()
    {
        string xml = PlayerPrefs.GetString("counters");
        StringReader stringReader = new StringReader(xml);
        counters = (XmlSerializableCounters)counterSerializer.Deserialize(stringReader);

        xml = PlayerPrefs.GetString("gameProgress");
        stringReader = new StringReader(xml);
        gameProgress = (HashSet<string>)hashSetSerializer.Deserialize(stringReader);

        float xPos = PlayerPrefs.GetFloat("PlaerPosX", playerSpawnPoint.x);
        float yPos = PlayerPrefs.GetFloat("PlaerPosY", playerSpawnPoint.y);
        playerSpawnPoint = new Vector2(xPos, yPos);
        currentLevelSceneCode = PlayerPrefs.GetInt("CurrLevel", currentLevelSceneCode);

    }

    public static void Clear()
    {
        gameProgress.Clear();
        counters.Clear();
        playerSpawnPoint = Vector2.zero;
        currentLevelSceneCode = 1;
    }

    public static void AddEvent(string eventCode)
    {
        gameProgress.Add(eventCode);
        Debug.Log("code:" + eventCode);
    }

    public static bool CheckCondition(string eventCode)
    {
        if(eventCode[0] == '!')
            return !gameProgress.Contains(eventCode.Remove(0,1));
        return gameProgress.Contains(eventCode);
    }

    public static void IncrementCounter(string counterCode)
    {
        Debug.Log("code:" + counterCode);
        if (counters.ContainsKey(counterCode))
        {
            counters[counterCode]++;
        }
        else
        {
            counters.Add(counterCode, 1);
        }
    }

    public static void ClearLevelData(string LevelCode)
    {
        string[] eventCodes = new string[gameProgress.Count];
        gameProgress.CopyTo(eventCodes);
        foreach(string eventCode in eventCodes)
        {
            if (eventCode.StartsWith(LevelCode)) gameProgress.Remove(eventCode);
        }
        string[] counterCodes = new string[counters.Count];
        counters.Keys.CopyTo(counterCodes,0);
        foreach (string key in counterCodes)
        {
            if (key.StartsWith(LevelCode))
            {
                counters.Remove(key);
            }
        }
    }

    public static int GetCounter(string counterCode)
    {
        int total = 0;
        foreach (string key in counters.Keys)
        {
            if (key.Contains(counterCode))
            {
                total += counters[key];
            }
        }

        return total;
    }
   
}
