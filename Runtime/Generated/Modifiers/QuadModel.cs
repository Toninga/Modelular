using UnityEngine;

namespace Modelular.Runtime
{
	public class QuadModel : ModifierModel
	{
		#region Fields

		public DefaultPrimitiveProperties DefaultParameters = DefaultPrimitiveProperties.Default();
        public Vector2 Size = Vector2.one;
        public Vector2Int Subdivisions = Vector2Int.zero;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private DefaultPrimitiveProperties _defaultParameters;
        private Vector2 _size;
        private Vector2Int _subdivisions;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public QuadModel()
		{
			underlyingModifier = new Modelular.Runtime.Quad();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Quad);
			target.DefaultParameters = DefaultParameters;
            target.Size = Size;
            target.Subdivisions = Subdivisions;
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
                 _subdivisions != Subdivisions ||
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
             _subdivisions = Subdivisions;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
