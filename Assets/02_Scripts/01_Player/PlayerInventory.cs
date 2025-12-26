using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    private PlayerAttack mPlayer;

    public List<ItemBase> Items = new List<ItemBase>();

    public Item_Equipment[] EquipmentSlot = new Item_Equipment[System.Enum.GetValues(typeof(ItemType)).Length];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
    }

    public void AddItem(ItemBase item, int amount = 1) 
    {
        ItemBase exsisting = null;

        //인벤에 존재하는 아이템찾기
        for (int i = 0; i < Items.Count; i++) 
        {
            ItemBase inventoryItem = Items[i];

            if (inventoryItem.ItemDataSO == item.ItemDataSO) 
            {
                exsisting = (ItemBase)inventoryItem;
                break;
            }
        }
        //같은 아이템 없으면 그냥 추가
        if (exsisting == null) 
        {
            Items.Add(item);
        }
        //있는데 맥스스택이 1이라 합칠 수 없으면
        else if (exsisting.ItemDataSO.MaxStack == 1)
        {  
            //중복이어도 추가
            Items.Add(item);
        }
        else
        {
            //합칠 수 있으면 합치기
            ChangeItemAmount(exsisting, amount);
        }
    }

    public void RemoveItem(ItemBase item) 
    {
        Items.Remove(item);
    }

    public void ChangeItemAmount(ItemBase item, int amount) 
    {
        item.ItemDataSO.CurrentStack += amount;

        //갯수가 0보다 작거나 같으면 리스트에서 제거
        if (item.ItemDataSO.CurrentStack <= 0) 
        {
            RemoveItem(item); 
        }
        //최대 스택 넘으면 최대 스택으로 깎아주기
        if (item.ItemDataSO.CurrentStack > item.ItemDataSO.MaxStack) 
        {
            item.ItemDataSO.CurrentStack = item.ItemDataSO.MaxStack;
        }
    }

    public void EquipItem(Item_Equipment equipment) 
    {
        ItemType type = equipment.ItemDataSO.ItemType;
        EquipmentSlot[(int)type] = equipment;
    }

    public void ApplyEquipmentEffect() 
    {
        foreach (Item_Equipment equipment in EquipmentSlot) 
        {
            if (equipment != null && equipment.ItemDataSO)
            {
                switch (equipment.ItemDataSO.ItemEffect) 
                {
                    case ItemEffect.HpIncrease:
                        break;
                    case ItemEffect.MoveSpeedIncrease:
                        break;
                    case ItemEffect.AttackIncrease:
                        break;
                    case ItemEffect.None:
                        break;
                }
            }
        }
    }
}
