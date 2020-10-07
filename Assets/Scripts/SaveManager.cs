using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager 
{
    private static HashSet<string> gameProgress = new HashSet<string>();
    private static Dictionary<string, int> counters = new Dictionary<string, int>();
    public static Vector2 playerSpawnPoint;
    public static int curruntLevelSceneCode = 1;

    public static void Clear()
    {
        gameProgress.Clear();
        counters.Clear();
        playerSpawnPoint = Vector2.zero;
        curruntLevelSceneCode = 1;
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
        foreach(string eventCode in gameProgress)
        {
            if (eventCode.StartsWith(LevelCode)) gameProgress.Remove(eventCode);
        }

        foreach (string key in counters.Keys)
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
