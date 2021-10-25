using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using Photon.Pun;

public class SerializationController : MonoBehaviour
{
    List<LineRenderer> lines = new List<LineRenderer>(); //опять же возможно стоит использовать GameObkect
    StringBuilder str = new StringBuilder();
    Serializer<LineRenderer> serializer = new Serializer<LineRenderer>(); 

    public void AddLine(LineRenderer line)
    {
        lines.Add(line); // Может использоваться в будущем для более удобного изменения(удаления) старых линий
        serializer.AddToSerializaiton(str, line);
    }

    /// <summary>
    /// Вызвать перед закрытием комнаты. меню?
    /// </summary>
    public void Serialize()
    {
        serializer.Serialise(lines);
    }

    public void Deserialize(GameObject brush)
    {
        var lines = serializer.Deserialize();
        foreach(var l in lines)
        {
            PhotonNetwork.Instantiate(brush.name, l.transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// NotImplemented
    /// </summary>
    public void ModifyLine(LineRenderer line)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// NotImplemented
    /// </summary>
    public void DeleteLine(LineRenderer line)
    {
        throw new NotImplementedException();
    }
}
