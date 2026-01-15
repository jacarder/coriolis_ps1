using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
	fileName = "NewCharacterStats",
	menuName = "RPG/Character Stats"
)]
public class CharacterStats : ScriptableObject
{
	[Header("Attributes")]
	public Attributes attributes = new();

	[Header("Skills")]
	[SerializeField]
	private List<Skill> skills = new();

	public IReadOnlyList<Skill> Skills => skills;

	private void OnEnable()
	{
		EnsureSkillsExist();
	}

	public int GetTotalDiceBySkill(SkillType type)
	{
		Skill skill = skills.Find(x => x.Type == type);
		return attributes.Get(skill.GoverningAttribute) + skill.value;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		EnsureSkillsExist();
	}
#endif

	private void EnsureSkillsExist()
	{
		if (skills == null)
			skills = new List<Skill>();

		foreach (SkillType type in System.Enum.GetValues(typeof(SkillType)))
		{
			if (!skills.Exists(s => s.Type == type))
			{
				skills.Add(new Skill(type));
			}
		}

		// Keep inspector order stable and readable
		skills.Sort((a, b) => a.Type.CompareTo(b.Type));
	}

	public Skill GetSkill(SkillType type)
	{
		return skills.Find(s => s.Type == type);
	}
}
