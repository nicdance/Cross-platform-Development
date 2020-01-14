using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paths : MonoBehaviour
{
    public Color pathColor = Color.green;
    public List<Transform> pathObjList = new List<Transform>();
    Transform[] objArray;

    [Range(1,20)]public int  lineDensity = 1;
    public List<Vector3> bezierObj = new List<Vector3>();
    int overload;

    public bool visualisePath = false;

    // Start is called before the first frame update
    void Start()
    {
        CreatePath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnDrawGizmos()
    {
        if (visualisePath)
        {
            // STRAIGHT PATH
            Gizmos.color = pathColor;
            // Fill the Array
            objArray = GetComponentsInChildren<Transform>();
            // Clear List
            pathObjList.Clear();
            // Put all children into list
            foreach (Transform obj in objArray)
            {
                if (obj != this.transform)
                {
                    pathObjList.Add(obj);
                }
            }
            // draw the object
            for (int i = 0; i < pathObjList.Count; i++)
            {
                Vector3 position = pathObjList[i].position;
                if (i > 0)
                {
                    Vector3 previous = pathObjList[i - 1].position;
                    Gizmos.DrawLine(previous, position);
                    Gizmos.DrawWireSphere(position, .3f);
                }
            }

            // CURVED PATH

            // CHECK FOR OVERLOAD
            if (pathObjList.Count % 2 == 0)
            {
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                overload = 2;
            }
            else
            {
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                overload = 3;
            }

            // CURVE Creation
            bezierObj.Clear();

            // setting start point
            Vector3 lineStart = pathObjList[0].position;

            for (int i = 0; i < pathObjList.Count - overload; i += 2)
            {
                for (int j = 0; j <= lineDensity; j++)
                {
                    Vector3 lineEnd = GetPoint(pathObjList[i].position,
                                               pathObjList[i + 1].position,
                                               pathObjList[i + 2].position,
                                               j / (float)lineDensity);


                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(lineStart, lineEnd);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(lineStart, .1f);
                    lineStart = lineEnd;
                    bezierObj.Add(lineStart);
                }
            }
        }
        else {
           // pathObjList.Clear();
           // bezierObj.Clear();
        }
      
    }
    

    Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float time)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, time), Vector3.Lerp(p1, p2, time), time);
    }

    void CreatePath() {
        // Fill the Array
        objArray = GetComponentsInChildren<Transform>();
        // Clear List
        pathObjList.Clear();
        // Put all children into list
        foreach (Transform obj in objArray)
        {
            if (obj != this.transform)
            {
                pathObjList.Add(obj);
            }
        }
        // CURVED PATH

        // CHECK FOR OVERLOAD
        if (pathObjList.Count % 2 == 0)
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 2;
        }
        else
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 3;
        }

        // CURVE Creation
        bezierObj.Clear();

        // setting start point
        Vector3 lineStart = pathObjList[0].position;

        for (int i = 0; i < pathObjList.Count - overload; i += 2)
        {
            for (int j = 0; j <= lineDensity; j++)
            {
                Vector3 lineEnd = GetPoint(pathObjList[i].position,
                                           pathObjList[i + 1].position,
                                           pathObjList[i + 2].position,
                                           j / (float)lineDensity);

                lineStart = lineEnd;
                bezierObj.Add(lineStart);
            }
        }
    }
}
