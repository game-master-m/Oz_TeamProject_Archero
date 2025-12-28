using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBaseSO", menuName = "Archero/ItemData/ItemDataBase")]
public class ItemDataBaseSO : ScriptableObject
{
    [SerializeField] private List<ItemDataSO> mItemDatabase;

    public List<ItemDataSO> ItemDatabase => mItemDatabase;
}
