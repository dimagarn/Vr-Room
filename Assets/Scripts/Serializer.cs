using System.Collections.Generic;
using System.IO;
using System.Text;
using Online.RoomDB;
using Valve.Newtonsoft.Json;
using UnityEngine;

public class Serializer<GameObject> //Можно ли сделать GameObject вместо TDrawing
{
    private string path; // Изменить -> Resourses?
    private IRoomDB roomDb;

    public Serializer(string room)
    {
        roomDb = new YandexRoomDB();
        path = @$"{Application.dataPath}\Serialized_{typeof(GameObject).Name}_{room}.txt";
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
            roomDb.SaveRoom(text);
            //sw.WriteLine(text);
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
            var serializedDrawings = roomDb.LoadRoom();
               // sw.ReadToEnd();
            if (serializedDrawings.Length != 0)
                lines = JsonConvert.DeserializeObject<List<GameObject>>(serializedDrawings);
        }
        
        return lines;
    }
}
