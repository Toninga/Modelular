using Modelular.Modifiers;
using UnityEngine;


namespace Modelular.Editor.Modifiers
{
    public abstract class ModifierModel : ScriptableObject
    {
        public bool enabled = true;
        [HideInInspector] public bool hasChanged = false;

        public Modifier underlyingModifier;

        public abstract void ApplyParameters();
        public void SetDirty(bool value=true) => hasChanged = value;
        public abstract bool DetectChanges();
    }

}