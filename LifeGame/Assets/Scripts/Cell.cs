using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3Int cellPos { get; private set; }
    public bool isAlived;

    public Cell(Vector3Int cellPos, bool isAlived)
    {
        this.cellPos = cellPos;
        this.isAlived = isAlived;
    }
}
