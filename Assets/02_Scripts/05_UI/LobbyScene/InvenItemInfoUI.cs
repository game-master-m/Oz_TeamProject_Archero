using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenItemInfoUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    //InventorySlotUI에서 ItemSlot 정보를 받는다
    [SerializeField] private ItemSlotEventChannelSO mOnInvenItemSelected;   //InventorySlotUI 가 발송

    [Header("인벤 아이템인포 컴포넌트 참조")]
    [SerializeField] private Transform mRoot_InvenItemInfo;
    [SerializeField] private TextMeshProUGUI mItemNameText;
    [SerializeField] private TextMeshProUGUI mItemDescText;
    [SerializeField] private TextMeshProUGUI mItemSellPriceText;
    [SerializeField] private TextMeshProUGUI mItemCountText;
    [SerializeField] private TextMeshProUGUI mItemTotalPriceText;
    [SerializeField] private Image mItemIcon;
    [SerializeField] private Button mEquipBtn;
    [SerializeField] private Button mSellBtn;
    [SerializeField] private Button mAddBtn;
    [SerializeField] private Button mSubBtn;
    [SerializeField] private Button mCloseBtn;
    [SerializeField] private Button mSelfCloseBtn;

    private ItemSlot mSlot;

    private string mItemName;
    private string mItemDesc;
    private int mItemSellPrice;
    private int mItemCount;
    private int mTotalPrice;
    private Sprite mIconSprite;

    private void OnEnable()
    {
        mOnInvenItemSelected.onEvent += HandleItemSelect;
        InitBtns();
    }

    private void OnDisable()
    {
        mOnInvenItemSelected.onEvent -= HandleItemSelect;
    }

    private void InitBtns()
    {
        mEquipBtn.onClick.RemoveAllListeners();
        mSellBtn.onClick.RemoveAllListeners();
        mAddBtn.onClick.RemoveAllListeners();
        mSubBtn.onClick.RemoveAllListeners();
        mCloseBtn.onClick.RemoveAllListeners();
        mSelfCloseBtn.onClick.RemoveAllListeners();

        mEquipBtn.onClick.AddListener(OnClickEquipBtn);
        mSellBtn.onClick.AddListener(OnClickSellBtn);
        mAddBtn.onClick.AddListener(OnClickAddBtn);
        mSubBtn.onClick.AddListener(OnClickSubBtn);
        mCloseBtn.onClick.AddListener(OnClickCloseBtn);
        mSelfCloseBtn.onClick.AddListener(OnClickCloseBtn);
    }
    private void HandleItemSelect(ItemSlot slot)
    {
        mSlot = slot;

        mItemName = mSlot.itemData.ItemName;
        mItemDesc = mSlot.itemData.Description;
        mItemSellPrice = mSlot.itemData.SellPrice;
        mItemCount = mSlot.currentStack;
        mTotalPrice = mItemCount * mItemSellPrice;
        mIconSprite = mSlot.itemData.ItemSprite;

        SetInvenItemInfoUI();

        mRoot_InvenItemInfo.gameObject.SetActive(true);
    }
    private void SetInvenItemInfoUI()
    {
        mItemNameText.SetText(Utils.StringAppend(mItemName));
        mItemDescText.SetText(Utils.StringAppend(mItemDesc));
        mItemSellPriceText.SetText(Utils.IntAppend(mItemSellPrice));
        mItemCountText.SetText(Utils.IntAppend(mItemCount));
        mItemTotalPriceText.SetText(Utils.IntAppend(mTotalPrice));

        mItemIcon.sprite = mIconSprite;
    }
    private void OnClickEquipBtn()
    {
        Managers.Data.EquipItem(mSlot);

        mRoot_InvenItemInfo.gameObject.SetActive(false);
    }
    private void OnClickSellBtn()
    {
        Managers.Data.SellItem(mSlot.itemData, mItemCount);

        mRoot_InvenItemInfo.gameObject.SetActive(false);
    }
    private void OnClickAddBtn()
    {
        if (mItemCount >= mSlot.currentStack) return;
        mItemCount++;
        UpdatePriceText();
    }
    private void OnClickSubBtn()
    {
        if (mItemCount <= 1) return;
        mItemCount--;
        UpdatePriceText();
    }
    private void UpdatePriceText()
    {
        mTotalPrice = mItemSellPrice * mItemCount;
        mItemCountText.SetText(Utils.IntAppend(mItemCount));
        mItemTotalPriceText.SetText(Utils.IntAppend(mTotalPrice));
    }
    private void OnClickCloseBtn()
    {
        mRoot_InvenItemInfo.gameObject.SetActive(false);
    }
}
