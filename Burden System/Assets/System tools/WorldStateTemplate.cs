using NoStudios.Burdens;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldStateSaveData", menuName = "Fault/WorldStateSaveData", order = 1)]
public sealed class WorldStateTemplate : DataTemplate<WorldState>
{
}


public class WorldState : ScriptableObject
{
    public int worldFlag = 0;
}