using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper 
{
    public static T[] GetJsonArray<T>(string json)
    {
        string newJson = "{ \"levels\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.levels;
    }

    public static string ConvertToJson<T>(T[] obj)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.levels = obj;
        string str = JsonUtility.ToJson(wrapper);
        return str;
    }
}


[System.Serializable]
public class Wrapper<T>
{
    public T[] levels;
}
