using UnityEngine;
using Modelular.Modifiers.Primitives;
using Modelular.Modifiers;
using Modelular.Selection;


namespace Modelular.Editor.Modifiers
{

    //[CreateAssetMenu(fileName = "CopyToRadius", menuName = "Modellular/Copy To Radius")]
    public class CopyToRadiusModel : ModifierModel
    {
        #region Fields

        public int Count = 8;
        public float Radius = 1f;
        public float Arc01 = 1f;
        public bool FaceNormal = true;
        //[Field]

        // Replicated fields for change detection
        private bool _enabled;
        private int _count;
        private float _radius;
        private float _arc01;
        private bool _faceNormal;
        //[ReplicatedField]

        #endregion

        public CopyToRadiusModel()
        {
            underlyingModifier = new Modelular.Modifiers.CopyToRadius();
        }

        public override void ApplyParameters()
        {
            var target = (underlyingModifier as Modelular.Modifiers.CopyToRadius);
            target.Count = Count;
            target.Radius = Radius;
            target.Arc01 = Arc01;
            target.FaceNormal = FaceNormal;
            //[SetProperty]
        }

        public override bool DetectChanges()
        {
            // Insert here the comparison for all properties that should be change-checked
            if 
            (
                enabled != _enabled ||
                 _count != Count ||
                 _radius != Radius ||
                 _arc01 != Arc01 ||
                 _faceNormal != FaceNormal ||
                //[ChangeCheck]
                false
            )
            {
                hasChanged = true;
            }

            // Reset the mirrored fields
            _enabled = enabled;
             _count = Count;
             _radius = Radius;
             _arc01 = Arc01;
             _faceNormal = FaceNormal;
            //[ReplicatedFieldReset]

            return hasChanged;
        }
    }

}
