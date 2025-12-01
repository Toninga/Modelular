using UnityEngine;
using Modelular.Modifiers.Primitives;
using Modelular.Modifiers;
using Modelular.Selection;


namespace Modelular.Editor.Modifiers
{

    //[CreateAssetMenu(fileName = "AttachModifierStack", menuName = "Modellular/Attach Modifier Stack")]
    public class AttachModifierStackModel : ModifierModel
    {
        #region Fields

        public ModularMesh LinkedMesh;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private ModularMesh _linkedMesh;
        //[ReplicatedField]

        #endregion

        public AttachModifierStackModel()
        {
            underlyingModifier = new Modelular.Modifiers.AttachModifierStack();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Modifiers.AttachModifierStack);
            target.LinkedMesh = LinkedMesh;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _linkedMesh != LinkedMesh ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _linkedMesh = LinkedMesh;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
