using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Formation : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeY = 2;

    public float gridOffsetX = 1;
    public int gridOffsetY = 1;

    public int divider = 4;

    public List<Vector3> gridList = new List<Vector3>();

    // MOVE Formation
    public float maxMoveOffsetX = 5;

    float curPosX;

    Vector3 startPosition;

    public float speed = 5f;
    int direction = -1;




    private void Start()
    {
        startPosition = this.transform.position;
        curPosX = transform.position.x;
        CreateGrid();

    }

    private void Update()
    {
        curPosX += Time.deltaTime * speed * direction;
        if (curPosX >= maxMoveOffsetX)
        {
            direction *= -1;
            curPosX = maxMoveOffsetX;
        }
        else if (curPosX <= -maxMoveOffsetX)
        {
            direction *= -1;
            curPosX = -maxMoveOffsetX;
        }
        transform.position = new Vector3(curPosX, startPosition.y, startPosition.z);

        
    }

    //private void OnDrawGizmos()
    //{
    //    int num = 0;
    //    CreateGrid();
    //    foreach (Vector3 item in gridList)
    //    {
    //        Gizmos.DrawWireSphere(GetVectorPosition(num), .1f);
    //        num++;
    //    }
    //}

    public void CreateGrid() {
        gridList.Clear();
        int num = 0;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                //float x = gridOffsetX * i;
                float x = (gridOffsetX + gridOffsetX * 2 * (num / divider)) * Mathf.Pow(-1, num % 2 + 1);
                //float y = gridOffsetY * j;
                float y = gridOffsetY * ((num % divider) / 2);

                //Vector3 vector = new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0);
                Vector3 vector = new Vector3(x, y, 0);
                num++;
                gridList.Add(vector);
            }
        }
    }
    public Vector3 GetVectorPosition(int EnemyId) {
        return transform.position + gridList[EnemyId];
    }

   
}
