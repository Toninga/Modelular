using Modelular.Runtime;


namespace Modelular.Runtime
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
            underlyingModifier = new Modelular.Runtime.FlipNormals();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Runtime.FlipNormals);
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
