using UnityEngine;


namespace Modelular.Runtime
{

    //[CreateAssetMenu(fileName = "SelectByColor", menuName = "Modellular/Select By Color")]
    public class SelectByColorModel : ModifierModel
    {
        #region Fields

        public string OutputSelectionGroup;
        public ESelectionOperand SelectionOperand;
        public Color Color = Color.white;
        public float Tolerance01;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private string _outputSelectionGroup;
        private ESelectionOperand _selectionOperand;
        private Color _color;
        private float _tolerance01;
        //[ReplicatedField]

        #endregion

        public SelectByColorModel()
        {
            underlyingModifier = new Modelular.Runtime.SelectByColor();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Runtime.SelectByColor);
            target.OutputSelectionGroup = OutputSelectionGroup;
            target.SelectionOperand = SelectionOperand;
            target.Color = Color;
            target.Tolerance01 = Tolerance01;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _outputSelectionGroup != OutputSelectionGroup ||
                 _selectionOperand != SelectionOperand ||
                 _color != Color ||
                 _tolerance01 != Tolerance01 ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _outputSelectionGroup = OutputSelectionGroup;
             _selectionOperand = SelectionOperand;
             _color = Color;
             _tolerance01 = Tolerance01;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
