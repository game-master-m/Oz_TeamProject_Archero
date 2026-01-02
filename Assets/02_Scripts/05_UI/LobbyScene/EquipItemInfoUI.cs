using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemInfoUI : MonoBehaviour
{
    [Header("이벤트 구독")]
    //EquipmentSlotUI에서 ItemDataSO 정보를 받는다
    [SerializeField] private ItemDataEventChannelSO mOnEquipItemSelected;   //EquipmentSlotUI 가 발송

    [Header("이벤트 발행")]
    [SerializeField] private EquipedItemDataEventChannelSO mOnEquipedItemData;  //DataManager도 발행 -> InventoryUI

    [Header("인벤 아이템인포 컴포넌트 참조")]
    [SerializeField] private Transform mRoot_EquipItemInfo;
    [SerializeField] private TextMeshProUGUI mItemNameText;
    [SerializeField] private TextMeshProUGUI mItemDescText;
    [SerializeField] private Image mItemIcon;
    [SerializeField] private Button mUnEquipBtn;
    [SerializeField] private Button mCloseBtn;
    [SerializeField] private Button mSelfCloseBtn;

    private ItemDataSO mItemData;

    private string mItemName;
    private string mItemDesc;
    private Sprite mIconSprite;

    private void OnEnable()
    {
        mOnEquipItemSelected.onEvent += HandleItemSelect;
        InitBtns();
    }

    private void OnDisable()
    {
        mOnEquipItemSelected.onEvent -= HandleItemSelect;
    }

    private void InitBtns()
    {
        mUnEquipBtn.onClick.RemoveAllListeners();
        mCloseBtn.onClick.RemoveAllListeners();
        mSelfCloseBtn.onClick.RemoveAllListeners();

        mUnEquipBtn.onClick.AddListener(OnClickUnEquipBtn);
        mCloseBtn.onClick.AddListener(OnClickCloseBtn);
        mSelfCloseBtn.onClick.AddListener(OnClickCloseBtn);
    }
    private void HandleItemSelect(ItemDataSO data)
    {
        mItemData = data;

        mItemName = data.ItemName;
        mItemDesc = data.Description;
        mIconSprite = data.ItemSprite;

        SetEquipItemInfoUI();

        mRoot_EquipItemInfo.gameObject.SetActive(true);
    }
    private void SetEquipItemInfoUI()
    {
        mItemNameText.SetText(Utils.StringAppend(mItemName));
        mItemDescText.SetText(Utils.StringAppend(mItemDesc));
        mItemIcon.sprite = mIconSprite;
    }
    private void OnClickUnEquipBtn()
    {
        Managers.Data.UnequipItem(mItemData.ItemType);
        mOnEquipedItemData?.Raised(Managers.Data.GetEquippedItems());
        mRoot_EquipItemInfo.gameObject.SetActive(false);

        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mEquipSound);
    }

    private void OnClickCloseBtn()
    {
        mRoot_EquipItemInfo.gameObject.SetActive(false);
    }
}
