using UnityEngine;

namespace Modelular.Runtime
{
	public class DiskModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters = DefaultPrimitiveProperties.Default();
        [Min(0f)]
        public float Radius = 0.5f;
        [Min(3f)]
        public int Subdivisions = 16;
        [Range(0f, 1f)]
        public float Arc = 1f;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private DefaultPrimitiveProperties _defaultParameters;
        private float _radius;
        private int _subdivisions;
        private float _arc;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public DiskModel()
		{
			underlyingModifier = new Modelular.Runtime.Disk();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Disk);
			target.DefaultParameters = DefaultParameters;
            target.Radius = Radius;
            target.Subdivisions = Subdivisions;
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
                 _radius != Radius ||
                 _subdivisions != Subdivisions ||
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
             _radius = Radius;
             _subdivisions = Subdivisions;
             _arc = Arc;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
