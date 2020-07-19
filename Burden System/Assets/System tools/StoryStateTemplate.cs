using System;
using NoStudios.Burdens;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryStateSaveData", menuName = "ScriptableObjects/StoryStateSaveData", order = 1)]
public class StoryStateTemplate : DataTemplate<StoryState>
{
}

[Serializable]
public class StoryState
{
    public int StoryFlag = 0;
}
