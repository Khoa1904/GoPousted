using CoreWCF.IdentityModel.Tokens;
using System;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace GoShared.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };

    public static (T model, string result) JsonToModel<T>(string jsonString) where T : class, new()
    {
        try
        {
            T? result = JsonSerializer.Deserialize<T>(jsonString, Options);
            if (result != null)
                return (result, "OK");

            LogHelper.Logger.Error("JsonToModel Error : Null");
            return (new T(), "ERROR");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("JsonToModel Error : " + ex.Message);
            return (new T(), "ERROR");
        }
    }
    public static (List<T> model, string result) JsonToModels<T>(string jsonString) where T : class, new()
    {
        try
        {
            List<T>? result = JsonSerializer.Deserialize<List<T>>(jsonString, Options);
            if (result != null)
                return (result, "OK");

            LogHelper.Logger.Error("JsonToModel Error : Null");
            return (new List<T>(), "ERROR");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("JsonToModel Error : " + ex.Message);
            return (new List<T>(), "ERROR");
        }
    }

    public static string ModelToJson<T>(T modelClass) where T : class
    {
        return JsonSerializer.Serialize(modelClass, Options);
    }

    public static string JsonPrettify(this string jsonString)
    {
        try
        {
            using JsonDocument jDoc = JsonDocument.Parse(jsonString);
            string str = JsonSerializer.Serialize(jDoc, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            return str;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public static string InfoToJson<T>(T modelClass) where T : class, new()
    {
        try
        {
            return JsonSerializer.Serialize(modelClass, Options);
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("JsonToModel Error : " + ex.Message);
            return "";
        }
    }
    public static string Info2Json(object modelClass)
    {
        try
        {
            return JsonSerializer.Serialize(modelClass, Options);
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("JsonToModel Error : " + ex.Message);
            return "";
        }
    }


}