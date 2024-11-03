using Newtonsoft.Json;

public static class Extension
{
    public static T ToObject<T>(this string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static string ToJson(this object data)
    {
        return JsonConvert.SerializeObject(data);
    }
}