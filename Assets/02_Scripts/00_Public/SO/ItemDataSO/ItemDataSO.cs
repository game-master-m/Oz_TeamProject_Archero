using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataSO", menuName = "Archero/ItemData/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public ItemBase ItemPrefab;
    public Sprite ItemSprite;

    public int MaxStack = 1;
    public int CurrentStack = 1;

    public float ItemCost;

    public string ItemName;
    public string Description;
}
