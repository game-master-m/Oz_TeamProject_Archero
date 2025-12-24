
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "Archero/DropData/DropTableSO")]
public class DropTableSO : ScriptableObject
{
    public List<DropItemData> commonDrops;
}
