using UnityEngine;

public static class Polyominos
{
    private static readonly int[][,] polyominos = new int[][,]
    {
        new int[,]
        {
            {0, 0, 1},
            {0, 0, 1 },
            {1, 1, 1 }
        },
        new int[,]
        {
            {1 },
            {1 }
        },
        new int[,]
        {
            {1 }
        },
        new int[,]
        {
            {0, 1 },
            {1, 0 }
        },
        new int[,]
        {
            {1, 1, 1 },
            {1, 1, 1 },
            {1, 1, 1 }
        },
        new int[,]
        {
            {1, 1, 1 }
        },
        new int[,]
        {
            {1, 1 },
            {1, 1 }
        },
        new int[,]
        {
            {0, 1, 1 },
            {1, 1, 0 }
        },
        new int[,]
        {
            {0, 1 },
            {1, 1 },
            {1, 0 }
        }
    };

    static Polyominos()
    {
        foreach (var polyomino in polyominos)
        {
            ReverseRows(polyomino);
        }
    }

    public static int[,] Get(int index) => polyominos[index];
    
    public static int Length => polyominos.Length;

    public static void ReverseRows(int[,] polyomino)
    {
        var polyominoRows = polyomino.GetLength(0);
        var polyominoCols = polyomino.GetLength(1);

        for (var r = 0; r < polyominoRows / 2; ++r)
        {
            for (var c = 0; c < polyominoCols; ++c)
            {
                var tmp = polyomino[r, c];
                polyomino[r, c] = polyomino[polyominoRows - r - 1, c];
                polyomino[polyominoRows - r - 1, c] = tmp;
            }
        }
    }
}
