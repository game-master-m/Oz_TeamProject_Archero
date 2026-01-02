using UnityEngine;

public class MonsterListManager : MonoBehaviour
{
    public MonsterImageSO imageSO;
    public GameObject mSlotPrefab;
    public Transform mBox;

    private void Start()
    {
        foreach(MonsterData data in imageSO.monsterImage)
        {
            GameObject go = Instantiate(mSlotPrefab, mBox);
            MonsterSlot monsterSlot=go.GetComponent<MonsterSlot>();
            if (monsterSlot != null)
            {
                monsterSlot.Setup(data);
            }
        }
    }
}
