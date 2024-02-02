using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class PlatformPathEditorOnly : MonoBehaviour
{
    public PlatformPath platformPath;

    public bool DrawPoints;
    public bool DrawLines;
    public bool DrawPointNumbers;

    GUIStyle style = new GUIStyle();



    [ExecuteAlways]
    private void OnEnable()
    {
        UpdatePathPoints();
        style.normal.textColor = Color.black;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 18;
    }

    [ExecuteAlways]
    private void OnDrawGizmos()
    {

        for (int i = 0; i < platformPath.pathPoints.Count; i++)
        {


            Transform curPoint = platformPath.pathPoints[i];

            if (DrawPoints)
            {

                Handles.color = Color.gray;
                Handles.DrawSolidDisc(curPoint.position, Vector3.back, 1);
            }

            if (DrawLines)
            {
                    
                Transform nextPoint = i < platformPath.pathPoints.Count - 1 ? platformPath.pathPoints[i + 1] : platformPath.pathPoints[0];

                Handles.color = Color.white;
                Handles.DrawLine(curPoint.position, nextPoint.position);
            }

            if (DrawPointNumbers)
            {
                float labelWidth = 1f;
                float labelHeight = 2f;
                Vector3 labelOffset = new Vector3(-labelWidth / 2, labelHeight / 2);
                Vector3 labelPosition = curPoint.position + labelOffset;

                Handles.Label(labelPosition, i.ToString(), style);
            }
        }
    }

    [ExecuteAlways]
    public void UpdatePathPoints()
    {
        platformPath.UpdatePathPoints();
    }
}
