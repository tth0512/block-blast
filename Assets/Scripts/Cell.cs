using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite normal;

    [SerializeField] private Sprite highlight;

    private Color cellColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetColor(CellColor color)
    {
        cellColor = AssetManager.Ins.GetColorMaterial(color);
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void Normal()
    {
        gameObject.SetActive(true);
        //spriteRenderer.color = Color.white;
        spriteRenderer.color = cellColor;
        spriteRenderer.sprite = normal;
    }

    public void Highlight(CellColor color)
    {
        //gameObject.SetActive(true);
        //spriteRenderer.color = Color.white;
        spriteRenderer.color = AssetManager.Ins.GetColorMaterial(color);
        spriteRenderer.sprite = highlight;
    }

    public void Hover()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = cellColor;
        spriteRenderer.color = new Color(cellColor.r, cellColor.g, cellColor.b, 0.5f);
        spriteRenderer.sprite = normal;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
