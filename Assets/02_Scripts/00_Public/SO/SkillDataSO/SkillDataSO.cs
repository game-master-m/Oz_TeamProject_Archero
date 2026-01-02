using UnityEngine;

public abstract class SkillDataSO : ScriptableObject
{
    [Header("UI 표시 정보")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
    public ESkillGrade skillGrade;
    [Header("스태킹 여부")]
    public bool isStacking = false;
    public virtual IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public virtual IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
