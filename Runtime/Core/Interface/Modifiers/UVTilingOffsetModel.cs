using UnityEngine;

namespace Modelular.Runtime
{

    //[CreateAssetMenu(fileName = "UVTilingOffset", menuName = "Modellular/UVTiling Offset")]
    public class UVTilingOffsetModel : ModifierModel
    {
        #region Fields

        public Vector2 Tiling = Vector2.one;
        public Vector2 Offset;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private Vector2 _tiling;
        private Vector2 _offset;
        //[ReplicatedField]

        #endregion

        public UVTilingOffsetModel()
        {
            underlyingModifier = new Modelular.Runtime.UVTilingOffset();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Runtime.UVTilingOffset);
            target.Tiling = Tiling;
            target.Offset = Offset;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _tiling != Tiling ||
                 _offset != Offset ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _tiling = Tiling;
             _offset = Offset;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
