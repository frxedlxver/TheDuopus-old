using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformPath : MonoBehaviour
{
    public List<Transform> pathPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
