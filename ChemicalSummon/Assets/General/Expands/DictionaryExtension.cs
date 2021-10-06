using System.Collections.Generic;

public static class DictionaryExtension
{
    public static void Put<keyT, valueT>(this Dictionary<keyT, valueT> dic, keyT key, valueT value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }
    }
}
