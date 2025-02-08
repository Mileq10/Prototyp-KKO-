using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [field:SerializeField]
    public PokerHand MinimumRank { get; set; } = 0;
    [field: SerializeField]
    public PokerHand CriticalRank { get; set; } = 0;
    [field: SerializeField]
    public GameObject Prefab { get; set; }
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    public int Health { get; internal set; }
}
