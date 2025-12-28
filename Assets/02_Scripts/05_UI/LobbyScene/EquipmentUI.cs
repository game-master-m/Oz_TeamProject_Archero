using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    private PlayerAttack mPlayer;
    private SlotUI mSlotUIPrefab;
    private List<SlotUI> mInventorySlots;
    [SerializeField] private SlotUI mArmorSlot;
    [SerializeField] private SlotUI mHelmetSlot;
    [SerializeField] private SlotUI mShoesSlot;
    [SerializeField] private SlotUI mWeaponSlot;

    private void OnEnable()
    {
        if (PlayerInventory.Instance == null) return;
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        Managers.Pool.CreatePool(mSlotUIPrefab, 60, Managers.Pool.transform);
        UpdateSlotImage();
    }

    public void GenerateInventoryUI() 
    {
        mInventorySlots = new List<SlotUI>();
        int inventoryCount = PlayerInventory.Instance.Items.Count;
        for (int i = 0; i < inventoryCount; i++) 
        {
            SlotUI slot = Managers.Pool.GetFromPool(mSlotUIPrefab);
            slot.SetItemData(PlayerInventory.Instance.Items[i]);
            mInventorySlots.Add(slot);
        }
    }

    public void UpdateSlotImage() 
    {
        mArmorSlot.SetItemData(PlayerInventory.Instance.EquipmentSlot[(int)EItemType.Armor]);
        mHelmetSlot.SetItemData(PlayerInventory.Instance.EquipmentSlot[(int)EItemType.Helmet]);
        mShoesSlot.SetItemData(PlayerInventory.Instance.EquipmentSlot[(int)EItemType.Shoes]);
        mWeaponSlot.SetItemData(PlayerInventory.Instance.EquipmentSlot[(int)EItemType.Weapon]);
    }
}
