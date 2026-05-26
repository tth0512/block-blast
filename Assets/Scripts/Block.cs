
using System;
using UnityEngine;
using UnityEngine.XR;

public class Block : MonoBehaviour
{
    public const int Size = 5;

    private readonly Vector3 inputOffset = new(0f, 2f, 0f);

    [SerializeField] private Blocks blocks;

    [SerializeField] private Board board;

    [SerializeField] private Cell cellPrefab;

    private int polyominoIndex;

    private readonly Cell[,] cells = new Cell[Size, Size];

    private Vector3 position;
    private Vector3 scale;

    private Vector2 inputPoint;

    private Vector3 previousMousePosition = Vector3.positiveInfinity;

    private Vector2Int previousDragPoint;

    private Vector2Int currentDragPoint;

    public CellColor CellsColor;

    //Cache 

    private Camera mainCamera;
    private Vector2 center;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void RandomColor()
    {
        CellsColor = Enum.GetValues(typeof(CellColor)).GetValue(UnityEngine.Random.Range(0, 3)) is CellColor color ? color : CellColor.Red;
    }
    public void Initialize()
    {
        RandomColor();
        Debug.Log($"Block color {CellsColor}");

        for (var r = 0; r < Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                cells[r, c] = Instantiate(cellPrefab, transform);
                cells[r, c].SetColor(CellsColor);
            }
        }

        position = transform.position;
        scale = transform.localScale;
    }

    public void Show(int polyominoIndex)
    {
        this.polyominoIndex = polyominoIndex;
        Hide();
        var polyomino = Polyominos.Get(polyominoIndex);
        var polynominoRows = polyomino.GetLength(0);
        var polynominoCols = polyomino.GetLength(1);

        for (var r = 0; r < polynominoRows; ++r)
        {
            for (var c = 0; c < polynominoCols; ++c)
            {
                if (polyomino[r, c] > 0)
                {
                    cells[r, c].transform.localPosition = new(c, r, 0f);
                    cells[r, c].Normal();
                }

            }
        }
    }

    private void Hide()
    {
        for (var r = 0; r < Size; ++r)
        {
            for (var c = 0; c < Size; ++c)
            {
                cells[r, c].Hide();
            }
        }
    }

    private void Highlight()
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        UnHighlight();

        var currentColor = InputManager.Ins.CurrentBlock.CellsColor;

        foreach (var row in board.highlightPolyRows)
        {
            var r = polyominoRows - row - 1;
            for (var col = 0; col < polyominoCols; ++col)
            {
                if (polyomino[row, col] > 0)
                {
                    cells[row, col].Highlight(currentColor);
                }
            }
        }

        foreach (var col in board.highlightPolyCols)
        {
            for (var row = 0; row < polyominoRows; ++row)
            {
                
                if (polyomino[row, col] > 0)
                {
                    cells[row, col].Highlight(currentColor);
                }
            }
        }
    }

    private void UnHighlight()
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        for (var row = 0; row < polyominoRows; ++row)
        {
            for (var col = 0; col < polyominoCols; ++col)
            {
                if (polyomino[row, col] > 0)
                {
                    cells[row, col].Normal();
                }
            }
        }

    }

    private void OnMouseDown()
    {
        Debug.Log("On Mouse Down");

        InputManager.Ins.CurrentBlock = this;
        inputPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.localPosition = position + inputOffset;
        transform.localScale = Vector3.one;

        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position);

        board.Hover(currentDragPoint, polyominoIndex);
        Debug.Log($"Drag point {currentDragPoint}");
        previousDragPoint = currentDragPoint;

        previousMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        var currentMousePosition = Input.mousePosition;
        if (currentMousePosition != previousMousePosition)
        {
            previousMousePosition = currentMousePosition;
            Debug.Log("On Mouse Drag");

            var inputDelta = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - inputPoint;
            transform.position = position + inputOffset + (Vector3)inputDelta;

            currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position);
            if (currentDragPoint != previousDragPoint)
            {
                board.Hover(currentDragPoint, polyominoIndex);
                Highlight();
                previousDragPoint = currentDragPoint;
                Debug.Log($"Drag point {currentDragPoint}");
            }
        }
    }

    private void OnMouseUp()
    {
        previousMousePosition = Vector3.positiveInfinity;

        currentDragPoint = Vector2Int.RoundToInt((Vector2)transform.position);
        if (board.Place(currentDragPoint, polyominoIndex, CellsColor))
        {
            gameObject.SetActive(false);
            blocks.Remove();
        }

        transform.localPosition = position;
        transform.localScale = scale;
    }

}
