using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInfoUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    //InventorySlotUI에서 ItemSlot 정보를 받는다
    [SerializeField] private ItemDataEventChannelSO mOnShopItemSelected;   //ShopSlotUI 가 발송

    [Header("인벤 아이템인포 컴포넌트 참조")]
    [SerializeField] private Transform mRoot_ShopItemInfo;
    [SerializeField] private TextMeshProUGUI mItemNameText;
    [SerializeField] private TextMeshProUGUI mItemDescText;
    [SerializeField] private TextMeshProUGUI mItemBuyPriceText;
    [SerializeField] private Image mItemIcon;
    [SerializeField] private Button mBuyBtn;
    [SerializeField] private Button mCloseBtn;
    [SerializeField] private Button mSelfCloseBtn;

    private ItemDataSO mItemData;

    private string mItemName;
    private string mItemDesc;
    private int mItemBuyPrice;
    private Sprite mIconSprite;

    private void OnEnable()
    {
        InitBtns();
        mOnShopItemSelected.onEvent += HandleItemSelect;
    }

    private void OnDisable()
    {
        mOnShopItemSelected.onEvent -= HandleItemSelect;
    }

    private void InitBtns()
    {
        mBuyBtn.onClick.RemoveAllListeners();
        mCloseBtn.onClick.RemoveAllListeners();
        mSelfCloseBtn.onClick.RemoveAllListeners();

        mBuyBtn.onClick.AddListener(OnClickBuyBtn);
        mCloseBtn.onClick.AddListener(OnClickCloseBtn);
        mSelfCloseBtn.onClick.AddListener(OnClickCloseBtn);
    }
    private void HandleItemSelect(ItemDataSO itemData)
    {
        mItemData = itemData;

        mItemName = itemData.ItemName;
        mItemDesc = itemData.Description;
        mItemBuyPrice = itemData.ItemCost;
        mIconSprite = itemData.ItemSprite;

        SetShopItemInfoUI();

        mRoot_ShopItemInfo.gameObject.SetActive(true);
    }
    private void SetShopItemInfoUI()
    {
        mItemNameText.SetText(Utils.StringAppend(mItemName));
        mItemDescText.SetText(Utils.StringAppend(mItemDesc));
        mItemBuyPriceText.SetText(Utils.IntAppend(mItemBuyPrice));

        mItemIcon.sprite = mIconSprite;
    }
    private void OnClickBuyBtn()
    {
        //인벤에 추가
        if (mItemBuyPrice > Managers.Data.Gold) return;
        Managers.Data.AddItemToInventory(mItemData, 1);
        Managers.Data.AddGold(-mItemBuyPrice);

        mRoot_ShopItemInfo.gameObject.SetActive(false);

        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mSellItemSound);
    }

    private void OnClickCloseBtn()
    {
        mRoot_ShopItemInfo.gameObject.SetActive(false);
    }
}
