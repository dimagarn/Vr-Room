using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class Serializer<TDrawing> //Можно ли сделать GameObject вместо TDrawing
{
    private string path; // Изменить -> Resourses?

    public Serializer(string room)
    {
        path = @$"Assets\Data\Serialized_{typeof(TDrawing).Name}_{room}.txt";
    }

    public Serializer(string path, string room)
    {
        path = @$"{path}\Serialized_{typeof(TDrawing).Name}_{room}.txt";
    }

    public void Serialise(List<TDrawing> drawing)
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

    public List<TDrawing> Deserialize()
    {
        var lines = new List<TDrawing>();
        if (!File.Exists(path))
            return lines;
        using (StreamReader sw = new StreamReader(path, Encoding.GetEncoding(1251)))
        {
            var serializedDrawings = sw.ReadToEnd();
            if (serializedDrawings.Length != 0)
                lines = JsonConvert.DeserializeObject<List<TDrawing>>(serializedDrawings);
        }
        return lines;
    }
}
