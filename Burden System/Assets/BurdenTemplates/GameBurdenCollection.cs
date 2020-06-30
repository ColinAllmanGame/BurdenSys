using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{

    [CreateAssetMenu(fileName = "DefaultBurdenContainer", menuName = "ScriptableObjects/MakeBurdenContainer", order = 1)]
    public class GameBurdenCollection : ScriptableObject
    {
        public Burden worldBurden1;
        public Burden worldBurden2;
        public Burden areaBurden1;
        public Burden areaBurden2;
        public Burden areaBurden3;
    }
}

