using UnityEngine;

namespace Modelular.Runtime
{
	public class CubeModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters = DefaultPrimitiveProperties.Default();
        public Vector3 Size = Vector3.one;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private DefaultPrimitiveProperties _defaultParameters;
        private Vector3 _size;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public CubeModel()
		{
			underlyingModifier = new Modelular.Runtime.Cube();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Cube);
			target.DefaultParameters = DefaultParameters;
            target.Size = Size;
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
                 _size != Size ||
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
             _size = Size;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
