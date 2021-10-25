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
    private string path = @$"Assets\Data\Serialized_{typeof(TDrawing).Name}.txt"; // Изменить -> Resourses?

    public Serializer(string path)
    {
        this.path = path;
    }

    public Serializer()
    {
    }

    public void AddToSerializaiton(StringBuilder str, TDrawing drawing)
        => str.Append(JsonConvert.SerializeObject(drawing));

    public void Serialise(List<TDrawing> drawing)
    {
        using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding(1251)))
        {
            sw.WriteLine(JsonConvert.SerializeObject(drawing));
        }
    }

    /// <summary>
    /// NotImplemented
    /// </summary>
    public void ReplaceDrawingInSerialization()
    {
        throw new NotImplementedException();
    }

    public List<TDrawing> Deserialize()
    {
        var lines = new List<TDrawing>();
        using (StreamReader sw = new StreamReader(path, Encoding.GetEncoding(1251)))
        {
            var serializedDrawings = sw.ReadToEnd();
            if (serializedDrawings.Length != 0)
                lines = JsonConvert.DeserializeObject<List<TDrawing>>(serializedDrawings);
        }
        return lines;
    }
}
