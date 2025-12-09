using UnityEngine;

public abstract class SkillDataSO : ScriptableObject
{
    [Header("UI 표시 정보")]
    public string skillName;
    public string description;
    public Sprite icon;
    public ESkillGrade skillGrade;

    public virtual IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public virtual IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
