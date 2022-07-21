using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [SerializeField] private float iterationsSpeed = 50;
    [SerializeField] private TileBase tile;
    [SerializeField] private Tilemap tilemap;

    private int directionsCount = 8; // Количество сторон, по которым работает игра
    private int countForBirth = 3;
    private List<int> countForLive = new List<int>() { 2, 3 };
    private InputController inputController;
    private List<Cell> cells = new List<Cell>();
    private List<Vector2Int> neighborDirections = new List<Vector2Int>()
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
    };

    void Awake()
    {
        inputController = GetComponent<InputController>();

        BuildGrid();
        StartCoroutine("NextIteration");
    }

    private void Update()
    {
        if (inputController.leftMouseButton)
            PaintCell(Color.gray);
        if (inputController.rightMouseButton)  
            PaintCell(Color.white);
    }

    private void PaintCell(Color color)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilemapPos = tilemap.WorldToCell(mousePos);
        Cell cell = cells.Find(x => x.cellPos == tilemapPos);

        if (color == Color.white) 
            cell.isAlived = false;
        else if(color == Color.gray)
            cell.isAlived = true;

        tilemap.SetTileFlags(tilemapPos, TileFlags.None);
        tilemap.SetColor(tilemapPos, color);
    }

    private void BuildGrid()
    {
        Vector3Int startPos = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)))* 2;
        Vector3Int endPos = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0))) * 2;

        for (int y = startPos.y; y <= endPos.y; y++)
        {
            for (int x = startPos.x; x <= endPos.x; x++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                cells.Add(new Cell(cellPos, false));
                tilemap.SetTile(cellPos, tile);
            }
        }
    }

    IEnumerator NextIteration()
    {
        foreach (Cell cell in cells)
        {
            CheckNeighborsCount(cell);
        }
        DrawCells();
        yield return new WaitForSeconds(5f / iterationsSpeed);
        StartCoroutine("NextIteration");
    }

    private void CheckNeighborsCount(Cell cell)
    {
        int neighborsCount = 0;
        Vector3Int cellPos = cell.cellPos;

        for(int i = 0; i < directionsCount; i++)
        {
            Vector3Int neighborPos = new Vector3Int(cellPos.x + neighborDirections[i].x, cellPos.y + neighborDirections[i].y, 0);
            if (tilemap.GetColor(neighborPos) == Color.gray)
                neighborsCount++;
        }

        ChangeCell(cell, neighborsCount);
    }

    private void ChangeCell(Cell cell, int neighborsCount)
    {
        if (neighborsCount == countForBirth && !cell.isAlived)
        {
            cell.isAlived = true;
            return;
        }
        foreach (int count in countForLive)
        {
            if (neighborsCount == count && cell.isAlived)
            {
                cell.isAlived = true;
                return;
            }
        }
        cell.isAlived = false;
    }

    private void DrawCells()
    {
        foreach (Cell cell in cells)
        {
            Vector3Int cellPos = cell.cellPos;
            if (cell.isAlived)
            {
                tilemap.SetTileFlags(cellPos, TileFlags.None);
                tilemap.SetColor(cellPos, Color.gray);
            }
            else
            {
                tilemap.SetTileFlags(cellPos, TileFlags.None);
                tilemap.SetColor(cellPos, Color.white);
            }
        }
    }

    public void ClearGrid()
    {
        foreach (Cell cell in cells)
        {
            cell.isAlived = false;
            tilemap.SetColor(cell.cellPos, Color.white);
        }
    }

    public void SetGameMode(int directionsCount)
    {
        this.directionsCount = directionsCount;
    }

    public void SetCountForBirth(string input)
    {
        int number;
        if (int.TryParse(input, out number))
        {
            if (number <= directionsCount && number > 0)
                countForBirth = number;
        }
    }

    public void SetSpeedIterations(float speed)
    {
        iterationsSpeed = speed;
    }
}
