using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    [Header("Database")]
    public List<ItemDataSO> itemDatabase;

    [Header("이벤트 구독")]
    //골드 체인지 (ExpProgress Controller)
    //경험치 획득 (ExpProgress에서 할거냐, EndUI에서 한방에 할거냐?)

    [Header("이벤트 발송")]
    //로비 Gold UI update
    //로비 Inventory UI update

    //저장 될 데이터
    private int mGold = 0;          //GoldChange event 구독해서 누적 획득, 상점 로직에서 Add or Sub
    private int mLobbyExp = 0;      //ExpChange event 구독해서 클리어패널 생성 시 누적
    private string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    //인벤토리 데이터
    private List<ItemSlot> mInventoryItemSlots = new List<ItemSlot>();  //인게임 획득 및 상점 구매/판매
    private Dictionary<ItemType, ItemDataSO> mEquipedItemDic = new Dictionary<ItemType, ItemDataSO>();

    private int mRequiredExpToLevelUp = 500;    //처음에 500, 레벨업 할 때마다 20% 증가
    private int mCurrentLevel = 1;


    private void Awake()
    {
        LoadGame(); // 시작 시 자동 로드
    }

    #region Save & Load
    [ContextMenu("Save Game")] // 에디터에서 테스트 가능하도록
    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.gold = mGold;
        data.lobbyExp = mLobbyExp;
        data.currentLevel = mCurrentLevel;

        // 인벤토리 변환 (ItemSlot -> ItemSlotData)
        foreach (var slot in mInventoryItemSlots)
        {
            data.inventorySlots.Add(new ItemSlotData
            {
                itemName = slot.itemData.ItemName,
                currentStack = slot.currentStack
            });
        }

        // 장착 아이템 변환 (Dictionary -> List)
        foreach (var pair in mEquipedItemDic)
        {
            data.equippedItems.Add(new EquippedItemData
            {
                type = pair.Key,
                itemName = pair.Value.ItemName
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Utils.Log($"저장 완료: {SavePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Utils.Log("저장 파일이 없습니다. 초기 상태로 시작합니다.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 기본 변수 복구
        mGold = data.gold;
        mLobbyExp = data.lobbyExp;
        mCurrentLevel = data.currentLevel;

        // 인벤토리 복구
        mInventoryItemSlots.Clear();
        foreach (var slotData in data.inventorySlots)
        {
            ItemDataSO so = itemDatabase.Find(x => x.ItemName == slotData.itemName);
            if (so != null)
            {
                mInventoryItemSlots.Add(new ItemSlot(so, slotData.currentStack));
            }
        }

        // 장착 아이템 복구
        mEquipedItemDic.Clear();
        foreach (var equipData in data.equippedItems)
        {
            ItemDataSO so = itemDatabase.Find(x => x.ItemName == equipData.itemName);
            if (so != null)
            {
                mEquipedItemDic.Add(equipData.type, so);
            }
        }
        Utils.Log("데이터 로드 성공");
    }

    // 앱 종료 시 자동 저장
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    #endregion

    #region 아이템 변동
    //아이템 추가
    public void AddItemToInventory(ItemDataSO item, int count)
    {
        int remainingCount = count;

        foreach (var slot in mInventoryItemSlots)
        {
            if (slot.itemData == item && !slot.IsFull)
            {
                remainingCount = slot.AddStack(remainingCount);
                if (remainingCount <= 0) break;
            }
        }

        while (remainingCount > 0)
        {
            int stackToAdd = Mathf.Min(remainingCount, item.MaxStack);
            mInventoryItemSlots.Add(new ItemSlot(item, stackToAdd));
            remainingCount -= stackToAdd;
        }

        //로비 Inventory UI update
    }
    public void SellItem(ItemDataSO item, int sellCount)
    {
        // 1. 해당 아이템의 총 보유 수량 확인
        int totalOwned = GetTotalItemCount(item);

        if (totalOwned < sellCount)
        {
            Utils.Log("보유 수량이 부족하여 판매할 수 없습니다.");
            return;
        }

        // 2. 실제 아이템 차감 로직 (뒤에서부터 제거하는 것이 리스트 관리상 유리함)
        int remainingToSell = sellCount;
        for (int i = mInventoryItemSlots.Count - 1; i >= 0; i--)
        {
            if (mInventoryItemSlots[i].itemData == item)
            {
                if (mInventoryItemSlots[i].currentStack <= remainingToSell)
                {
                    // 슬롯의 수량보다 팔 양이 많거나 같으면 슬롯 제거
                    remainingToSell -= mInventoryItemSlots[i].currentStack;
                    mInventoryItemSlots.RemoveAt(i);
                }
                else
                {
                    // 슬롯의 수량이 더 많으면 수량만 차감
                    mInventoryItemSlots[i].currentStack -= remainingToSell;
                    remainingToSell = 0;
                }
            }

            if (remainingToSell <= 0) break;
        }

        // 3. 골드 정산
        int totalProfit = item.SellPrice * sellCount;
        AddGold(totalProfit);

        // 4. 알림
        //로비 Inventory UI update
    }

    public int GetTotalItemCount(ItemDataSO item)
    {
        int count = 0;
        foreach (var slot in mInventoryItemSlots)
        {
            if (slot.itemData == item) count += slot.currentStack;
        }
        return count;
    }
    #endregion

    #region 골드 변동
    //골드 추가
    public void AddGold(int amount)
    {
        mGold += amount;

        //로비 Gold UI update
    }
    //아이템 판매
    #endregion

    #region 아이템 장착 및 해제
    public void EquipItem(ItemSlot slot)
    {
        ItemDataSO itemToEquip = slot.itemData;

        // 1. 장착 가능한 타입인지 체크 (Etc 제외)
        if (itemToEquip.ItemType == ItemType.Etc)
        {
            Utils.Log($"{itemToEquip.ItemName}은 장착할 수 없는 아이템입니다.");
            return;
        }

        // 2. 해당 부위에 이미 장착된 아이템이 있는지 확인
        if (mEquipedItemDic.ContainsKey(itemToEquip.ItemType))
        {
            // 기존 장착 해제 및 인벤토리 복구
            UnequipItem(itemToEquip.ItemType);
        }

        // 3. 인벤토리에서 해당 아이템 수량 1 감소 (인벤토리 로직 활용)
        // 수량이 1개 이상일 때만 장착 가능
        RemoveItemFromInventory(itemToEquip, 1);

        // 4. 장착 딕셔너리에 추가
        mEquipedItemDic[itemToEquip.ItemType] = itemToEquip;

        Utils.Log($"{itemToEquip.ItemName}을 {itemToEquip.ItemType} 슬롯에 장착했습니다.");

        // UI 업데이트 알림 발송 필요
        // OnInventoryChanged?.Invoke();
        // OnEquipmentChanged?.Invoke();
    }

    public void UnequipItem(ItemType type)
    {
        if (!mEquipedItemDic.ContainsKey(type)) return;

        ItemDataSO unequippedItem = mEquipedItemDic[type];

        // 1. 딕셔너리에서 제거
        mEquipedItemDic.Remove(type);

        // 2. 인벤토리에 다시 추가
        AddItemToInventory(unequippedItem, 1);

        Utils.Log($"{unequippedItem.ItemName} 장착을 해제했습니다.");
    }

    private void RemoveItemFromInventory(ItemDataSO item, int count)
    {
        int remainingToRemove = count;
        for (int i = mInventoryItemSlots.Count - 1; i >= 0; i--)
        {
            if (mInventoryItemSlots[i].itemData == item)
            {
                if (mInventoryItemSlots[i].currentStack <= remainingToRemove)
                {
                    remainingToRemove -= mInventoryItemSlots[i].currentStack;
                    mInventoryItemSlots.RemoveAt(i);
                }
                else
                {
                    mInventoryItemSlots[i].currentStack -= remainingToRemove;
                    remainingToRemove = 0;
                }
            }
            if (remainingToRemove <= 0) break;
        }
    }
    #endregion

    public Dictionary<ItemType, ItemDataSO> GetEquippedItems()
    {
        return mEquipedItemDic;
    }
}

#region 저장 용 Serializable 클래스
[System.Serializable]
public class SaveData
{
    public int gold;
    public int lobbyExp;
    public int currentLevel;

    // 리스트는 직렬화 가능하지만, SO는 ID(string)로 저장해야 함
    public List<ItemSlotData> inventorySlots = new List<ItemSlotData>();
    public List<EquippedItemData> equippedItems = new List<EquippedItemData>();
}

[System.Serializable]
public class ItemSlotData
{
    public string itemName; // ItemDataSO의 고유 ID 혹은 이름
    public int currentStack;
}

[System.Serializable]
public class EquippedItemData
{
    public ItemType type;
    public string itemName;
}
#endregion

#region ItemSlotClass
[System.Serializable]
public class ItemSlot
{
    public ItemDataSO itemData;
    public int currentStack;

    public ItemSlot(ItemDataSO data, int count)
    {
        itemData = data;
        currentStack = count;
    }

    // 슬롯에 여유 공간이 있는지 확인
    public bool IsFull => currentStack >= itemData.MaxStack;

    // 남은 수량만큼 추가하고, 넘치는 양을 반환
    public int AddStack(int amount)
    {
        int available = itemData.MaxStack - currentStack;
        int toAdd = Mathf.Min(available, amount);

        currentStack += toAdd;
        return amount - toAdd; // 추가하지 못하고 남은 수량 반환
    }
}
#endregion