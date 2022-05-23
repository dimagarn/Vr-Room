using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class Serializer<GameObject> //Можно ли сделать GameObject вместо TDrawing
{
    private string path; // Изменить -> Resourses?

    public Serializer(string room)
    {
        path = Application.dataPath + "/StreamingAssets";
        if (Directory.Exists( Application.dataPath + "/StreamingAssets") == false)
        {
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        }
    }

    public Serializer(string path, string room)
    {
        path = @$"{path}\Serialized_{typeof(GameObject).Name}_{room}.txt";
    }

    public void Serialise(List<GameObject> drawing)
    {
        var setting = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding(1251)))
        {
            var text = JsonConvert.SerializeObject(drawing, setting);
            sw.WriteLine(text);
        }
    }

    public void DeleteSerialization()
    {
        File.Delete(path);
    }

    public List<GameObject> Deserialize()
    {
        var lines = new List<GameObject>();
        if (!File.Exists(path))
            return lines;
        using (StreamReader sw = new StreamReader(path, Encoding.GetEncoding(1251)))
        {
            var serializedDrawings = sw.ReadToEnd();
            if (serializedDrawings.Length != 0)
                lines = JsonConvert.DeserializeObject<List<GameObject>>(serializedDrawings);
        }
        return lines;
    }
}
