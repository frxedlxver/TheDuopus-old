using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovingPlatformController))]
[CanEditMultipleObjects]

public class MovingPlatform_Inspector : Editor
{
    private MovingPlatformController movingPlatform;
    private PlatformPath path;
    private GUIStyle headerStyle;

    private bool expandPointsFoldout = true;
    private bool expandVisualizationFoldout = true;


    private void OnEnable()
    {
        movingPlatform = (MovingPlatformController)target;
        path = movingPlatform.Path;
    }
    public override void OnInspectorGUI()
    {
        headerStyle = new(GUI.skin.label) {fontStyle = FontStyle.Bold, alignment = TextAnchor.LowerLeft};

        DrawDefaultInspector();

        EditorGUILayout.Space();
        
        DrawVisualizationOptions();
        
        EditorGUILayout.Space();

        DrawPathEditor();


        if (GUI.changed)
        {
            EditorUtility.SetDirty(movingPlatform);
        }
    }

    private void DrawVisualizationOptions()
    {

        expandVisualizationFoldout = EditorGUILayout.Foldout(expandVisualizationFoldout, "Path Visualization Options", true);

        EditorGUI.indentLevel++;
        EditorGUILayout.BeginVertical(GUI.skin.box);


        path.DrawPoints = EditorGUILayout.Toggle("Show path points", path.DrawPoints);

        if (path.DrawPoints) {
            
            path.DrawPointNumbers = EditorGUILayout.Toggle("Show point numbers", path.DrawPointNumbers);
        } else
        {
            path.DrawPointNumbers = false;
        }

        path.DrawLines = EditorGUILayout.Toggle("Show connecting lines", path.DrawLines);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private void DrawPathEditor()
    {
        expandPointsFoldout = EditorGUILayout.Foldout(expandPointsFoldout, "Path Points", true);
        if (expandPointsFoldout && movingPlatform.Path != null && path.pathPoints != null)
        {
            EditorGUI.indentLevel++;
            DrawAddPointButton();
            DrawPathPoints();
            EditorGUI.indentLevel--;
        }
    }

    private void DrawAddPointButton()
    {
        if (GUILayout.Button("Add Point"))
        {
            AddPointToPath();
        }
    }

    private void DrawPathPoints()
    {
        for (int i = 0; i < path.pathPoints.Count; i++)
        {
            DrawPathPoint(i);
        }
    }

    private void DrawPathPoint(int index)
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField($"Point {index}", headerStyle);

        path.pathPoints[index] = (Transform)EditorGUILayout.ObjectField("", path.pathPoints[index], typeof(Transform), true);

        if (path.pathPoints[index] != null)
        {
            Vector3 oldPosition = path.pathPoints[index].position;
            Vector3 newPosition = EditorGUILayout.Vector2Field("", oldPosition);
            if (newPosition != oldPosition)
            {
                Undo.RecordObject(path.pathPoints[index], "Move Path Point");
                path.pathPoints[index].position = new Vector3 (newPosition.x, newPosition.y, 0);
                EditorUtility.SetDirty(path.pathPoints[index]);
            }
        }

        DrawPathPointButtons(index);
        EditorGUILayout.EndVertical();
    }

    private void DrawPathPointButtons(int index)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("↑", GUILayout.Width(25)) && index > 0)
        {
            SwapPathPointOrder(index, index - 1);
        }
        if (GUILayout.Button("↓", GUILayout.Width(25)) && index < path.pathPoints.Count - 1)
        {
            SwapPathPointOrder(index, index + 1);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("remove", GUILayout.Width(60)))
        {
            RemovePointFromPath(index);
        }
        EditorGUILayout.EndHorizontal();
    }
    private void AddPointToPath()
    {
        // Create a new GameObject as a point and add it to the pathPoints list
        GameObject point = new GameObject($"Point {path.pathPoints.Count}");
        point.transform.SetParent(path.transform);
        path.pathPoints.Add(point.transform);
        path.UpdatePathPoints(); // Ensure the path points are updated
    }

    private void RemovePointFromPath(int index)
    {
        GameObject toRemove = path.pathPoints[index].gameObject;
        // Remove point GameObject when removing from list
        if (toRemove != null)
        {
            path.pathPoints.Remove(toRemove.transform);
            DestroyImmediate(toRemove);
        }
        path.UpdatePathPoints(); // Ensure the path points are updated
    }

    private void SwapPathPointOrder(int indexA, int indexB)
    {
        var temp = path.pathPoints[indexA];
        path.pathPoints[indexA] = path.pathPoints[indexB];
        path.pathPoints[indexB] = temp;
        path.UpdatePathPoints(); // Ensure the path points are updated after swap
    }
}
