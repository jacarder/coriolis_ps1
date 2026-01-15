[System.Serializable]
public class Attributes
{
    public int strength;
    public int agility;
    public int wits;
    public int empathy;

    public int Get(AttributeType type)
    {
        return type switch
        {
            AttributeType.Strength => strength,
            AttributeType.Agility => agility,
            AttributeType.Wits => wits,
            AttributeType.Empathy => empathy,
            _ => 0
        };
    }
}
