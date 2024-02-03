using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlatformPath : MonoBehaviour
{
    public List<Transform> pathPoints;

    public void UpdatePathPoints()
    {
        pathPoints = GetComponentsInChildren<Transform>().ToList();
        
        pathPoints.Remove(this.transform);
    }

    internal Transform GetNextPoint(Transform curPoint)
    {
        if (curPoint == null) return pathPoints[0];

        int curPointIdx = pathPoints.IndexOf(curPoint);
        return curPointIdx == pathPoints.Count - 1 ? pathPoints[0] : pathPoints[curPointIdx + 1];
    }

#if UNITY_EDITOR

    [Header("visualization")]
    public bool DrawPoints;
    public bool DrawLines;
    public bool DrawPointNumbers;

    private GUIStyle style = new();



    [ExecuteAlways]
    private void OnEnable()
    {
        UpdatePathPoints();
        style.normal.textColor = Color.black;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 18;
    }

    private void Update()
    {
        UpdatePathPoints();
    }

    [ExecuteAlways]
    private void OnDrawGizmos()
    {

        for (int i = 0; i < pathPoints.Count; i++)
        {


            Transform curPoint = pathPoints[i];

            if (DrawPoints)
            {

                Handles.color = Color.gray;
                Handles.DrawSolidDisc(curPoint.position, Vector3.back, 1);
            }

            if (DrawLines)
            {

                Transform nextPoint = i < pathPoints.Count - 1 ? pathPoints[i + 1] : pathPoints[0];

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

    #endif

}
