using UnityEngine;

namespace Modelular.Runtime
{
	public class AttachMeshModel : ModifierModel
	{
		#region Fields

		public STransform Transform = STransform.Default();
        public Mesh Mesh;
        public SelectorParameters OutputParameters;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private STransform _transform;
        private Mesh _mesh;
        private SelectorParameters _outputParameters;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public AttachMeshModel()
		{
			underlyingModifier = new Modelular.Runtime.AttachMesh();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.AttachMesh);
			target.Transform = Transform;
            target.Mesh = Mesh;
            target.OutputParameters = OutputParameters;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _transform != Transform ||
                 _mesh != Mesh ||
                 _outputParameters != OutputParameters ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _transform = Transform;
             _mesh = Mesh;
             _outputParameters = OutputParameters;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
