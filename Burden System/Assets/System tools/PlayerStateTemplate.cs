using System;
using NoStudios.Burdens;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateTemplate", menuName = "Fault/Save Data/Player State", order = 1)]
public sealed class PlayerStateTemplate : DataTemplate<PlayerState>
{
}

[Serializable]
public class PlayerState
{
    public int playerFlag = 0;
}
