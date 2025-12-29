using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopSlotUI slotPrefab;
    [SerializeField] private Transform mParent;

    //데이터 매니저한테 모든 아이템 리스트 받고
    private List<ItemDataSO> mItemDatabaseList;
    private void Awake()
    {
        if (Managers.Pool != null)
        {
            Managers.Pool.CreatePool(slotPrefab, 20, Managers.Pool.transform);
        }
        if (Managers.Data != null)
        {
            mItemDatabaseList = Managers.Data.ItemDatabaseList;
        }
        if (mItemDatabaseList != null)
        {
            foreach (ItemDataSO item in mItemDatabaseList)
            {
                ShopSlotUI slot = Managers.Pool.GetFromPool(slotPrefab);
                slot.SetShopData(item, mParent);
            }
        }
    }
}
