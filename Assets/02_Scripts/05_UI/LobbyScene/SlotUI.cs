using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SlotUI : MonoBehaviour
{
    [SerializeField] protected Button mIconBtn;
    [SerializeField] protected TextMeshProUGUI mStackText;

    protected ItemSlot mSlot;

    protected virtual void Awake()
    {
        mIconBtn.onClick.RemoveAllListeners();
        mIconBtn.onClick.AddListener(OnButtonClick);
    }
    public virtual void OnButtonClick()
    {
        SoundManager.Instance.BtnSound();
    }

    public virtual void SetItemData(ItemSlot slot, Transform parent)
    {
        mSlot = slot;
        mIconBtn.image.sprite = mSlot.itemData.ItemSprite;
        mStackText.SetText(Utils.IntAppend(mSlot.currentStack));
        transform.SetParent(parent);
    }

    public void ReturnToPool()
    {
        Managers.Pool.ReturnToPool(this);
    }
}
