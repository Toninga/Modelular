using UnityEngine;
using Modelular.Modifiers.Primitives;
using Modelular.Modifiers;
using Modelular.Selection;


namespace Modelular.Editor.Modifiers
{

    //[CreateAssetMenu(fileName = "Transform", menuName = "Modellular/Transform")]
    public class TransformModel : ModifierModel
    {
        #region Fields

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;
        //[ReplicatedField]

        #endregion

        public TransformModel()
        {
            underlyingModifier = new Modelular.Modifiers.Transform();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Modifiers.Transform);
            target.Position = Position;
            target.Rotation = Rotation;
            target.Scale = Scale;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _position != Position ||
                 _rotation != Rotation ||
                 _scale != Scale ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _position = Position;
             _rotation = Rotation;
             _scale = Scale;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
