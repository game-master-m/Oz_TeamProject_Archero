using UnityEngine;

public abstract class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;

    public virtual IProjectileStrategy CreateProjectileStrategy()
    {
        return null;
    }

    public virtual IPassiveStrategy CreatePassiveStrategy()
    {
        return null;
    }
}
