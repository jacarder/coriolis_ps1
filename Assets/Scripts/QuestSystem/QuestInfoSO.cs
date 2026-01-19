using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
	[field: SerializeField] public string id { get; private set; }
	[Header("General")]
	public string displayName;
	[Header("Requirements")]
	public QuestInfoSO[] questPrerequites;
	[Header("Steps")]
	public GameObject[] questStepPrefabs;
	[Header("Rewards")]
	public GameObject[] reward;
	public int experience;
	private void OnValidate()
	{
#if UNITY_EDITOR
		id = this.name;
		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}
}
