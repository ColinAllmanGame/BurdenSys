using NoStudios.Burdens;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldStateTemplate", menuName = "Fault/Save Data/World State", order = 1)]
public sealed class WorldStateTemplate : DataTemplate<WorldState>
{
}


[System.Serializable]
public class WorldState
{
    public int worldFlag = 0;
}