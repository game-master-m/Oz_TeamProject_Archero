using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataSO", menuName = "Archero/ItemData/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public Sprite ItemSprite;

    public EItemType ItemType;
    public EItemEffect ItemEffect;
    public float EffectAmount;

    public int MaxStack = 1;

    public float ItemCost;

    public string ItemName;
    public string Description;
}
