using UnityEngine;

namespace Modelular.Runtime
{
	public class CopyToRadiusModel : ModifierModel
	{
		#region Fields

		public int Count = 8;
        public float Radius = 1f;
        [Range(0f, 1f)]
        public float Arc01 = 1f;
        public bool FaceNormal = true;
        public EAxis Axis = EAxis.Y;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private int _count;
        private float _radius;
        private float _arc01;
        private bool _faceNormal;
        private EAxis _axis;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public CopyToRadiusModel()
		{
			underlyingModifier = new Modelular.Runtime.CopyToRadius();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.CopyToRadius);
			target.Count = Count;
            target.Radius = Radius;
            target.Arc01 = Arc01;
            target.FaceNormal = FaceNormal;
            target.Axis = Axis;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
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
                 _axis != Axis ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
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
             _axis = Axis;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
