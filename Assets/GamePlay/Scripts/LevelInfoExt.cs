using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public partial class LevelInfo
{
    public static bool pIsReady;

    private static List<LevelInfo> mLevelInfo = new List<LevelInfo>();

    public static void Init(string txt)
    {
        LevelInfo[] levelInfo = JsonHelper.GetJsonArray<LevelInfo>(txt);
        mLevelInfo = levelInfo.ToList<LevelInfo>();
        pIsReady = (mLevelInfo != null);
    }

    public static LevelInfo GetLevelInfo(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex > mLevelInfo.Count - 1)
            return null;

        return mLevelInfo[levelIndex];
    }

    public static int GetLevelCount()
    {
        return mLevelInfo.Count;
    }

    public static void UpdateLevelInfo(int levelIndex, int score, int stars)
    {
        /*if(mLevelInfo[levelIndex]._IsLocked)
        {
            Debug.LogError("Can't update level info because it is locked");
            return;
        }

        mLevelInfo[levelIndex]._Score = score;
        mLevelInfo[levelIndex]._Stars = stars;*/
        SaveLevelInfo();
    }

    public static void SaveLevelInfo()
    {
        LevelInfo[] levelInfo = mLevelInfo.ToArray<LevelInfo>();
        string levelJson = JsonHelper.ConvertToJson<LevelInfo>(levelInfo);
        PlayerPrefs.SetString("LevelInfo", levelJson);
        //File.WriteAllText(Application.dataPath + "/Common/LevelInfo.json", levelJson);
    }
}
