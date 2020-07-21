using System;
using NoStudios.Burdens;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryStateTemplate", menuName = "Fault/Save Data/Story State", order = 1)]
public class StoryStateTemplate : DataTemplate<StoryState>
{
}

[Serializable]
public class StoryState
{
    public int StoryFlag = 0;
}
