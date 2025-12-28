using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    //인벤토리 업데이트
    [SerializeField] private EquipedItemDataEventChannelSO mOnEquipedItemData;  //DataManager 발송
    [SerializeField] private ItemSlotsEventChannelSO mOnInvenItemSlots;         //DataManager 발송

    [Header("슬롯 프리팹")]
    [SerializeField] private InventorySlotUI mInvenSlotPrefab;
    [SerializeField] private Transform mInvenSlotParent;

    [Header("컴포넌트 참조")]
    [SerializeField] private EquipmentSlotUI mArmorSlot;
    [SerializeField] private EquipmentSlotUI mHelmetSlot;
    [SerializeField] private EquipmentSlotUI mShoesSlot;
    [SerializeField] private EquipmentSlotUI mWeaponSlot;

    private List<InventorySlotUI> mActiveInvenSlots = new List<InventorySlotUI>();
    private Dictionary<EItemType, EquipmentSlotUI> mEquipedSlotUIDic = new Dictionary<EItemType, EquipmentSlotUI>();

    private void Awake()
    {
        mEquipedSlotUIDic = new Dictionary<EItemType, EquipmentSlotUI>
        {
            { EItemType.Weapon, mWeaponSlot },
            { EItemType.Helmet, mHelmetSlot },
            { EItemType.Armor, mArmorSlot },
            { EItemType.Shoes, mShoesSlot }
        };

        if (Managers.Pool != null)
        {
            Managers.Pool.CreatePool(mInvenSlotPrefab, 40, Managers.Pool.transform);
        }
    }

    private void OnEnable()
    {
        mOnEquipedItemData.onEvent += UpdateEquipedSlots;
        mOnInvenItemSlots.onEvent += UpdateInventorySlots;

        if (Managers.Data != null)
        {
            UpdateEquipedSlots(Managers.Data.GetEquippedItems());
            UpdateInventorySlots(Managers.Data.GetInventoryItems());
        }
    }
    private void OnDisable()
    {
        mOnEquipedItemData.onEvent -= UpdateEquipedSlots;
        mOnInvenItemSlots.onEvent -= UpdateInventorySlots;

        foreach (var slot in mActiveInvenSlots)
        {
            if (Managers.Pool != null)
            {
                slot.transform.SetParent(Managers.Pool.transform);
                slot.ReturnToPool();
            }
        }
    }
    private void UpdateEquipedSlots(Dictionary<EItemType, ItemDataSO> equipedItemData)
    {
        if (equipedItemData == null) return;

        foreach (var pair in mEquipedSlotUIDic)
        {
            if (equipedItemData.TryGetValue(pair.Key, out ItemDataSO data))
            {
                pair.Value.SetEquipData(data);
            }
            else
            {
                pair.Value.SetEquipData(null);
            }
        }
    }

    private void UpdateInventorySlots(List<ItemSlot> invenItemList)
    {
        // 1. 기존에 활성화된 슬롯들을 모두 Pool로 반납
        foreach (var slot in mActiveInvenSlots)
        {
            slot.transform.SetParent(Managers.Pool.transform);
            slot.ReturnToPool();
        }
        mActiveInvenSlots.Clear();

        // 2. 새로운 리스트를 바탕으로 슬롯 활성화
        if (invenItemList == null) return;

        foreach (var itemSlot in invenItemList)
        {
            // Pool에서 꺼내기
            InventorySlotUI newSlot = Managers.Pool.GetFromPool(mInvenSlotPrefab);

            // 데이터 설정
            newSlot.SetItemData(itemSlot, mInvenSlotParent);

            // 추적 리스트에 추가
            mActiveInvenSlots.Add(newSlot);
        }
    }
}
