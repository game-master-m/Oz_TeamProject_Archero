using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataSO", menuName = "Archero/ItemData/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public Sprite ItemSprite;

    public ItemType ItemType;
    public ItemEffect ItemEffect;
    public float EffectAmount;

    public int MaxStack = 1;
    public int CurrentStack = 1;

    public int ItemCost;
    public int SellPrice;

    public string ItemName;
    public string Description;
}
