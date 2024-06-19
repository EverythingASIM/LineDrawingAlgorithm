using System;
using System.Collections.Generic;
using UnityEngine;

public class Bresenham : MonoBehaviour
{
    [SerializeField] GameObject quad;
    [SerializeField] GameObject lineList;

    MeshRenderer renderer;
    Texture2D lineDrawnTexture;

    public int Resolution = 16;
    void Start()
    {
        lineDrawnTexture = new Texture2D(Resolution, Resolution);
        lineDrawnTexture.filterMode = FilterMode.Point;

        renderer = quad.GetComponent<MeshRenderer>();
        renderer.material.mainTexture = lineDrawnTexture;
    }

    void Update()
    {
        List<Vector3> lines = new();
        for (int i = 0; i < lineList.transform.childCount; i++)
        {
            lines.Add(lineList.transform.GetChild(i).transform.position);
        }

        for (int i = 0; i < Resolution; i++)
        {
            for (int j = 0; j < Resolution; j++)
            {
                lineDrawnTexture.SetPixel(i, j, Color.black);
            }
        }

        var scale = quad.transform.lossyScale;
        var stepX = scale.x / Resolution;
        var stepY = scale.y / Resolution;

        for (int i = 0; i < lines.Count; i++)
        {
            var p1 = lines[i];
            var p2 = lines[(i + 1)%lines.Count];

            p1.x = p1.x / stepX;
            p2.x = p2.x / stepX;
            p1.y = p1.y / stepY;
            p2.y = p2.y / stepY;

            LineDrawing.Bresenham((int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y, lineDrawnTexture);
        }

        lineDrawnTexture.Apply();
    }

    

    void OnDrawGizmos()
    {
        var scale = quad.transform.lossyScale;

        var startPos = quad.transform.position - scale / 2;
        startPos.z = 0;

        Gizmos.color = Color.green;
        for (int i = 0; i < Resolution + 1; i++)
        {
            Vector3 start = startPos + new Vector3(i / (float)Resolution * scale.x, 0,0);
            Vector3 end = start + new Vector3(0, scale.y, 0);
            Gizmos.DrawLine(start, end);
        }
        for (int j = 0; j < Resolution + 1; j++)
        {
            Vector3 start = startPos + new Vector3(0, j / (float)Resolution * scale.y, 0);
            Vector3 end = start + new Vector3(scale.x, 0, 0);
            Gizmos.DrawLine(start, end);
        }
    }

    void OnGUI()
    {
        //GUI.
    }
}
