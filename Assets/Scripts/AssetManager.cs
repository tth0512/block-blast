using UnityEngine;

public class AssetManager : MonoBehaviour
{
    #region Instance
    private static AssetManager _instance;
    public static AssetManager Ins
    {
        get { return _instance; }
    }
    #endregion

    public BlockSpritesData blockSpritesData;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }        
        else
        {
            _instance = this;
        }

        blockSpritesData = Resources.Load<BlockSpritesData>("BlocksData/BlockSpritesData");
    }

    public Color GetColorMaterial(CellColor color)
    {
        return blockSpritesData.blockSprites[color];
    }

}
