using UnityEngine;

namespace Modelular.Runtime
{
	public class ConeModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters;
        [Min(0f)]
        public float BaseRadius = 0.5f;
        [Min(0f)]
        public float Height = 1f;
        public int FaceCount = 16;
        [Range(0f, 1f)]
        public float Fill = 1f;
        [Range(0f, 1f)]
        public float Arc = 1f;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private DefaultPrimitiveProperties _defaultParameters;
        private float _baseRadius;
        private float _height;
        private int _faceCount;
        private float _fill;
        private float _arc;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public ConeModel()
		{
			underlyingModifier = new Modelular.Runtime.Cone();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Cone);
			target.DefaultParameters = DefaultParameters;
            target.BaseRadius = BaseRadius;
            target.Height = Height;
            target.FaceCount = FaceCount;
            target.Fill = Fill;
            target.Arc = Arc;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _defaultParameters != DefaultParameters ||
                 _baseRadius != BaseRadius ||
                 _height != Height ||
                 _faceCount != FaceCount ||
                 _fill != Fill ||
                 _arc != Arc ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _defaultParameters = DefaultParameters;
             _baseRadius = BaseRadius;
             _height = Height;
             _faceCount = FaceCount;
             _fill = Fill;
             _arc = Arc;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
