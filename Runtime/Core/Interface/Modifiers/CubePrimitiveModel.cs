using UnityEngine;
using Modelular.Modifiers.Primitives;
using Modelular.Modifiers;
using Modelular.Selection;


namespace Modelular.Editor.Modifiers
{

    //[CreateAssetMenu(fileName = "CubePrimitive", menuName = "Modellular/Cube Primitive")]
    public class CubePrimitiveModel : ModifierModel
    {
        #region Fields

        public Color Color = Color.white;
        public string OutputSelectionGroup;
        public Vector3 Size = Vector3.one;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private Color _color;
        private string _outputSelectionGroup;
        private Vector3 _size;
        //[ReplicatedField]

        #endregion

        public CubePrimitiveModel()
        {
            underlyingModifier = new Modelular.Modifiers.Primitives.CubePrimitive();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Modifiers.Primitives.CubePrimitive);
            target.Color = Color;
            target.OutputSelectionGroup = OutputSelectionGroup;
            target.Size = Size;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _color != Color ||
                 _outputSelectionGroup != OutputSelectionGroup ||
                 _size != Size ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _color = Color;
             _outputSelectionGroup = OutputSelectionGroup;
             _size = Size;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
