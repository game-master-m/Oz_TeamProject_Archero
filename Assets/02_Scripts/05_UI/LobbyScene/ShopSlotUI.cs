using UnityEngine;

public class ShopSlotUI : SlotUI
{
    //발송...
    [SerializeField] private ItemDataEventChannelSO mOnShopItemSelected;   //ShopItemInfoUI 가 구독

    private ItemDataSO mItemData;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void SetItemData(ItemSlot slot, Transform parent) { }

    public void SetShopData(ItemDataSO data, Transform parent)
    {
        mItemData = data;

        mStackText.SetText("");
        mIconBtn.image.sprite = data.ItemSprite;
        transform.SetParent(parent);
    }
    public override void OnButtonClick()
    {
        //아이템 인포 패널 활성화
        mOnShopItemSelected?.Raised(mItemData);
    }

}
