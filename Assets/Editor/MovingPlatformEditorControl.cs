using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

[ExecuteInEditMode]
[RequireComponent(typeof(MovingPlatformController))]
public class MovingPlatformEditorControl : MonoBehaviour
{
    public MovingPlatformController movingPlatform;

    [ExecuteInEditMode]
    void Update()
    {
        if (!Application.isPlaying)
        {
            Vector3 platformPosition = movingPlatform.Platform.transform.position;
            Vector3 firstPointPosition = movingPlatform.Path.pathPoints[0].transform.position;
            if (platformPosition != firstPointPosition)
            {
                movingPlatform.Platform.transform.position = firstPointPosition;
            }
        }

    }
}
