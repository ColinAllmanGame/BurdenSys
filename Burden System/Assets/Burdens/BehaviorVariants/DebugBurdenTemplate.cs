using System;
using NoStudios.Burdens;
using UnityEngine;

namespace Burdens.BehaviorVariants
{
    [CreateAssetMenu(fileName = "DebugBurdenTemplate", menuName = "Fault/Burdens/Debug Burden")]
    public class DebugBurdenTemplate : BurdenTemplate<DebugBurden>
    {
        public BurdenTemplate WrappedBurden;

        public override DebugBurden Create(string sourceNote = null)
        {
            var clone = base.Create(sourceNote);
            clone.WrappedBurden = WrappedBurden.Clone(sourceNote);
            return clone;
        }
    }
    
    [Serializable]
    public class DebugBurden : Burden
    {
        [SerializeReference] public Burden WrappedBurden;
        
        public override int Fear => WrappedBurden.Fear * 2;

        public override int Hate => WrappedBurden.Hate / 2;

        public override int Regret => WrappedBurden.Regret;
        public override int Trauma => 1;

        public override Burden GenerateClone(string sourceNote = "")
        {
            var wrappedBurden = WrappedBurden;
            WrappedBurden = null;
            var clone = CloneFrom(this, sourceNote);
            WrappedBurden = wrappedBurden;
            clone.WrappedBurden = WrappedBurden;

            return clone;
        }
    }
}
