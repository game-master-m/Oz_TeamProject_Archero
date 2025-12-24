using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropItemData
{
    public ItemBase itemPrefab;
    [Range(0f, 1f)] //드랍확률은 0~1사이
    public float dropChance;
}
