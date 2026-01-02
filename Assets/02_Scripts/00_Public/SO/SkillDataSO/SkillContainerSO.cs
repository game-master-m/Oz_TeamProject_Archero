using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillContainerSO", menuName = "Archero/SkillData/SkillContainerSO")]
public class SkillContainerSO : ScriptableObject
{
    [SerializeField] private List<SkillDataSO> allSkills;

    public List<SkillDataSO> AllSkills => allSkills;
}
