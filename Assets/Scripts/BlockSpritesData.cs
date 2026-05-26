using UnityEngine;
[CreateAssetMenu(fileName = "BlockSpritesData", menuName = "ScriptableObjects/BlockSpritesData", order = 1)]
public class BlockSpritesData : ScriptableObject
{
    public UDictionary<CellColor, Color> blockSprites;
}
