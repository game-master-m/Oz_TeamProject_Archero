using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public string MonsterName;
    [TextArea] public string MonsterDescription;
    public Sprite MonsterIcon;
}



[CreateAssetMenu(fileName = "MonsterImageSO", menuName = "Archero/MonsterImageSO")]
public class MonsterImageSO : ScriptableObject
{
    
    public List<MonsterData> monsterImage=new List<MonsterData>();
}
