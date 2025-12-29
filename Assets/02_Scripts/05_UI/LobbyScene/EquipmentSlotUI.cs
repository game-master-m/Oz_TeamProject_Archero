using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : SlotUI
{
    [SerializeField] private Image mBackImage;

    //이벤트 발행
    [SerializeField] private ItemDataEventChannelSO mOnEquipItemSelected;

    private ItemDataSO mItemData;
    protected override void Awake()
    {
        base.Awake();
        mStackText.enabled = false;
    }
    public override void SetItemData(ItemSlot slot, Transform parent) { }

    public void SetEquipData(ItemDataSO itemData)
    {
        mItemData = itemData;
        if (itemData == null)
        {
            mBackImage.enabled = true;
            mIconBtn.enabled = false;
            mIconBtn.image.enabled = false;
        }
        else
        {
            mBackImage.enabled = false;
            mIconBtn.enabled = true;
            mIconBtn.image.enabled = true;
            mIconBtn.image.sprite = itemData.ItemSprite;
        }
    }
    public override void OnButtonClick()
    {
        Utils.Log("버튼 클릭!");
        mOnEquipItemSelected?.Raised(mItemData);
    }
}
