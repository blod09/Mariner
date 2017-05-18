public enum SkillType
{
    Woodwork,
    Paint,
    Fabric
}

public class Skill
{
    public SkillType Type { get; private set; }

    public int SkillLevel { get; set; }

    public Skill (SkillType type)
    {
        Type = type;
        SkillLevel = 0;
    }

}