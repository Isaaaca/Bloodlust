using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager 
{
    private static HashSet<string> gameProgress = new HashSet<string>();

    public static void AddEvent(string eventCode)
    {
        gameProgress.Add(eventCode);
    }

    public static bool CheckCondition(string eventCode)
    {
        return gameProgress.Contains(eventCode);
    }
   
}
