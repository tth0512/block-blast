using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Size = 8;

    [SerializeField] private Cell cellPrefab;

    [SerializeField] private Transform cellsTransform;

    private readonly Cell[,] cells = new Cell[Size, Size];
    private readonly int[,] data = new int[Size, Size];

    private readonly List<Vector2Int> hoverPoints = new();
    private readonly List<Vector2Int> polyPoints = new();

    private List<int> fullBlockRow = new List<int>();
    private List<int> fullBlockCol = new List<int>();

    public int[] numBlocksRow = new int[Size];
    public int[] numBlocksCol = new int[Size];

    public List<int> highlightPolyCols = new();
    public List<int> highlightPolyRows = new();

    private void Start()
    {
        for (var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                cells[r, c] = Instantiate(cellPrefab, cellsTransform);
                cells[r, c].transform.position = new(c, r, 0f);
                cells[r, c].Hide();
            }
        }
    }

    private void HoverPoints(Vector2Int point, int polyominoRows, int polyominoColumns, int[,] polyomino)
    {
        for (var r = 0; r < polyominoRows; ++r)
        {
            for (var c = 0; c < polyominoColumns; ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    var hoverPoint = point + new Vector2Int(c, r);
                    if (IsValidPoint(hoverPoint) == false)
                    {
                        hoverPoints.Clear();
                        return;
                    }

                    polyPoints.Add(new(r, c));
                    hoverPoints.Add(hoverPoint);
                }
            }
        }
    }

    private bool IsValidPoint(Vector2Int point)
    {
        if (point.x < 0 || point.x >= Size) return false;
        if (point.y < 0 || point.y >= Size) return false;
        if (data[point.y, point.x] > 0) return false;

        return true;
    }

    public void Hover(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        UnHover();
        UnHighLight();

        HoverPoints(point, polyominoRows, polyominoCols, polyomino);
        

        if (hoverPoints.Count > 0)
        {
            Hover();

            foreach (var row in fullBlockRow)
            {
                highlightPolyRows.Add(row - point.y);
            }
            foreach (var col in fullBlockCol)
            {
                if (col - point.x == 3)
                {
                    Debug.Log($"bug col: {col}, {point.x}");
                }
                highlightPolyCols.Add(col - point.x);
            }
        }
    }

    private void Hover()
    {
        for (int i = 0; i < hoverPoints.Count; ++i)
        {
            var hoverPoint = hoverPoints[i];
            var polyPoint = polyPoints[i];

            data[hoverPoint.y, hoverPoint.x] = 1;
            cells[hoverPoint.y, hoverPoint.x].Hover();

            numBlocksCol[hoverPoint.x]++;
            numBlocksRow[hoverPoint.y]++;

            if (numBlocksCol[hoverPoint.x] == 8)
            {
                HighLightCol(hoverPoint.x);
                fullBlockCol.Add(hoverPoint.x);
            }

            if (numBlocksRow[hoverPoint.y] == 8)
            {
                HighLightRow(hoverPoint.y);
                fullBlockRow.Add(hoverPoint.y);
            }
        }

        foreach (var hoverPoint in hoverPoints)
        {
            numBlocksRow[hoverPoint.y]--;
            numBlocksCol[hoverPoint.x]--;
        }
    }

    private void HighLightRow(int r)
    {
        for (var c = 0; c < Size; ++c)
        {
            cells[r, c].Highlight();
        }
        
    }

    private void HighLightCol(int c)
    {
        for (var r = 0; r < Size; ++r)
        {
            cells[r, c].Highlight();
        }
    }

    private void UnHighLight()
    {
        for (var r = 0; r < Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                if (data[r, c] == 2)
                {
                    cells[r, c].Normal();
                }
            }
        }
    }

    private void UnHover()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 0;
            cells[hoverPoint.y, hoverPoint.x].Hide();
        }

        hoverPoints.Clear();
        fullBlockCol.Clear();
        fullBlockRow.Clear();
        highlightPolyCols.Clear();
        highlightPolyRows.Clear();
    }

    public bool Place(Vector2Int point, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        UnHover();
        HoverPoints(point, polyominoRows, polyominoCols, polyomino);

        if (hoverPoints.Count > 0)
        {
            Place();
            return true;
        }

        return false;
    }

    private void Place()
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 2;
            cells[hoverPoint.y, hoverPoint.x].Normal();
            numBlocksRow[hoverPoint.y]++;
            numBlocksCol[hoverPoint.x]++;
            
            if (numBlocksRow[hoverPoint.y] == 8) DeleteRow(hoverPoint.y);
            if (numBlocksCol[hoverPoint.x] == 8) DeleteCol(hoverPoint.x);
        }

        hoverPoints.Clear();
        fullBlockCol.Clear();
        fullBlockRow.Clear();
    }

    private void DeleteRow(int r)
    {
        numBlocksRow[r] = 0;
        for (var c = 0; c < Size; ++c)
        {
            data[r, c] = 0;
            cells[r, c].Hide();
            numBlocksCol[c]--;
        }
    }

    private void DeleteCol(int c)
    {
        numBlocksCol[c] = 0;
        for (var r = 0; r < Size; ++r)
        {
            data[r, c] = 0;
            cells[r, c].Hide();
            numBlocksRow[r]--;
        }
    }

}
