using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    private int blockCount = 0;

    private void Start()
    {
        var blockWidth = (float)Board.Size / blocks.Length;
        var cellSize = (float)Board.Size / (Block.Size * blocks.Length + blocks.Length + 1);

        for (var i = 0; i < blocks.Length; i++)
        {
            blocks[i].transform.localPosition = new(blockWidth * (i + 0.25f), -0.25f - cellSize * 5f, 0f);
            blocks[i].transform.localScale = new(cellSize, cellSize, cellSize);
            blocks[i].Initialize();
        }

        Generate();
    }

    private void Generate()
    {
        for (var i = 0; i < blocks.Length; ++i)
        {
            blocks[i].gameObject.SetActive(true);
            int ranInt = Random.Range(0, Polyominos.Length);
            blocks[i].Show(ranInt);

            ++blockCount;
        }
    }

    public void Remove()
    {
        --blockCount;
        if (blockCount <= 0)
        {
            blockCount = 0;
            Generate();
        }
    }
}
