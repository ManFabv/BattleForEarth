using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type", menuName = "Config/Enemy type")]
public class EnemyConfig : ScriptableObject
{
    public TypeEnum.Option type;
}