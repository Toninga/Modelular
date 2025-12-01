using UnityEngine;


namespace Modelular.Runtime
{

    //[CreateAssetMenu(fileName = "SetColor", menuName = "Modellular/Set Color")]
    public class SetColorModel : ModifierModel
    {
        #region Fields

        public Color Color = Color.white;
        public string TargetSelectionGroup;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private Color _color;
        private string _targetSelectionGroup;
        //[ReplicatedField]

        #endregion

        public SetColorModel()
        {
            underlyingModifier = new Modelular.Runtime.SetColor();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Runtime.SetColor);
            target.Color = Color;
            target.TargetSelectionGroup = TargetSelectionGroup;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _color != Color ||
                 _targetSelectionGroup != TargetSelectionGroup ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _color = Color;
             _targetSelectionGroup = TargetSelectionGroup;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
