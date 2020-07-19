using System;
using NoStudios.Burdens;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateSaveData", menuName = "ScriptableObjects/PlayerStateSaveData", order = 1)]
public sealed class PlayerStateTemplate : DataTemplate<PlayerState>
{
    
}

[Serializable]
public class PlayerState
{
    public int playerFlag = 0;
}
