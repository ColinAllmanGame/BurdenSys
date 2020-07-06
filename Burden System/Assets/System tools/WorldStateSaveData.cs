using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldStateParmObject", menuName = "ScriptableObjects/MakeWorldStateObject", order = 1)]
public class WorldStateSaveData : ScriptableObject
{
    public int worldFlag = 0;
}

[CreateAssetMenu(fileName = "StoryStateParmObject", menuName = "ScriptableObjects/MakeWorldStateObject", order = 1)]
public class StoryStateSaveData : ScriptableObject
{
    public int StoryFlag = 0;
}

[CreateAssetMenu(fileName = "PlayerStateParmObject", menuName = "ScriptableObjects/MakeWorldStateObject", order = 1)]
public class PlayerStateSaveData : ScriptableObject
{
    public int playerFlag = 0;
}
