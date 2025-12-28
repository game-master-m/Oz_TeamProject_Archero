using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : SlotUI
{
    [SerializeField] private Image mBackImage;

    private void Awake()
    {
        mStackText.enabled = false;
    }
    public override void SetItemData(ItemSlot slot, Transform parent) { }

    public void SetEquipData(ItemDataSO itemData)
    {
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

    }
}
