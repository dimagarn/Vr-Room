using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using Photon.Pun;

public class SerializationController : MonoBehaviour
{
    List<Vector3[]> lines = new List<Vector3[]>(); //����� �� �������� ����� ������������ GameObkect
    StringBuilder str = new StringBuilder();
    Serializer<Vector3[]> serializer = new Serializer<Vector3[]>("noRoom"); 

    public void AddLine(Vector3[] line)
    {
        lines.Add(line); // ����� �������������� � ������� ��� ����� �������� ���������(��������) ������ �����
    }

    /// <summary>
    /// ������� ����� ��������� �������. ����?
    /// </summary>
    public void Serialize()
    {
        serializer.Serialise(lines);
    }

    public void DeleteSerialization()
    {
        serializer.DeleteSerialization();
    }

    public void Deserialize(GameObject brush)
    {
        var linesPositions = serializer.Deserialize();
        foreach(var positions in linesPositions)
        {
            var newLine = PhotonNetwork.Instantiate(brush.name, new Vector3(), Quaternion.identity);
            var drawLine = newLine.GetComponent<LineRenderer>();
            drawLine.positionCount = positions.Length;
            drawLine.SetPositions(positions);
            lines.Add(positions);
        }
    }
}
