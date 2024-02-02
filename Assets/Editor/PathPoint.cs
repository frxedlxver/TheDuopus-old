using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PathPoint : MonoBehaviour
{
    public PlatformPathEditorOnly parent;

    [ExecuteAlways]
    private void OnEnable()
    {
        parent = GetComponentInParent<PlatformPathEditorOnly>();
        parent.UpdatePathPoints();
    }

    [ExecuteAlways]
    private void OnDisable()
    {
        parent.UpdatePathPoints();
    }

    [ExecuteAlways]
    private void OnDestroy()
    {
        parent.UpdatePathPoints();
    }
}
