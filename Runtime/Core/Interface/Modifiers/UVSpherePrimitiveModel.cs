using UnityEngine;
using Modelular.Modifiers.Primitives;
using Modelular.Modifiers;
using Modelular.Selection;


namespace Modelular.Editor.Modifiers
{

    //[CreateAssetMenu(fileName = "UVSpherePrimitive", menuName = "Modellular/UVSphere Primitive")]
    public class UVSpherePrimitiveModel : ModifierModel
    {
        #region Fields

        public Color Color = Color.white;
        public string OutputSelectionGroup;
        public float Radius = 0.5f;
        public int HorizontalSubdivisions = 8;
        public int VerticalSubdivisions = 16;
        public string TargetSelectionSet;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private Color _color;
        private string _outputSelectionGroup;
        private float _radius;
        private int _horizontalSubdivisions;
        private int _verticalSubdivisions;
        private string _targetSelectionSet;
        //[ReplicatedField]

        #endregion

        public UVSpherePrimitiveModel()
        {
            underlyingModifier = new Modelular.Modifiers.Primitives.UVSpherePrimitive();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Modifiers.Primitives.UVSpherePrimitive);
            target.Color = Color;
            target.OutputSelectionGroup = OutputSelectionGroup;
            target.Radius = Radius;
            target.HorizontalSubdivisions = HorizontalSubdivisions;
            target.VerticalSubdivisions = VerticalSubdivisions;
            target.TargetSelectionSet = TargetSelectionSet;
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
                 _radius != Radius ||
                 _horizontalSubdivisions != HorizontalSubdivisions ||
                 _verticalSubdivisions != VerticalSubdivisions ||
                 _targetSelectionSet != TargetSelectionSet ||
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
             _radius = Radius;
             _horizontalSubdivisions = HorizontalSubdivisions;
             _verticalSubdivisions = VerticalSubdivisions;
             _targetSelectionSet = TargetSelectionSet;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
