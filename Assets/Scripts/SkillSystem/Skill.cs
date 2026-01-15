using UnityEngine;

[System.Serializable]
public class Skill
{
	[SerializeField]
	private SkillType type;

	public int value;

	public SkillType Type => type;

	public Skill(SkillType type)
	{
		this.type = type;
		value = 0;
	}

	public bool IsAdvanced => type >= SkillType.Command;

	public AttributeType GoverningAttribute
	{
		get
		{
			switch (type)
			{
				case SkillType.Dexterity:
				case SkillType.Infiltration:
				case SkillType.RangedCombat:
				case SkillType.Pilot:
					return AttributeType.Agility;

				case SkillType.Force:
				case SkillType.MeleeCombat:
					return AttributeType.Strength;

				case SkillType.Manipulation:
				case SkillType.Command:
				case SkillType.Culture:
				case SkillType.MysticPowers:
					return AttributeType.Empathy;

				default:
					return AttributeType.Wits;
			}
		}
	}
}
