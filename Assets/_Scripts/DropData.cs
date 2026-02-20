using UnityEngine;

[CreateAssetMenu(fileName = "NewDropData", menuName = "Game/Drop Data")]
public class DropData : ScriptableObject
{
    [Header("Drop Configuration")]

    [Range(0f, 100f)]
    public float dropChancePercent = 15f;

    public GameObject dropPrefab;
}
