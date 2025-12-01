using UnityEngine;
using Modelular.Modifiers.Primitives;
using Modelular.Modifiers;
using Modelular.Selection;


namespace Modelular.Editor.Modifiers
{

    //[CreateAssetMenu(fileName = "FlipNormals", menuName = "Modellular/Flip Normals")]
    public class FlipNormalsModel : ModifierModel
    {
        #region Fields

        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        //[ReplicatedField]

        #endregion

        public FlipNormalsModel()
        {
            underlyingModifier = new Modelular.Modifiers.FlipNormals();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Modifiers.FlipNormals);
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
