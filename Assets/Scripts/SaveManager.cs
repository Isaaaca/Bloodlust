using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager 
{
    private static HashSet<string> gameProgress = new HashSet<string>();
    public static Vector2 playerSpawnPoint;

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
   
}
