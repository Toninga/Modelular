using UnityEngine;

namespace Modelular.Runtime
{
	public class FlipModel : ModifierModel
	{
		#region Fields

		public string TargetSelectionGroup;
        public bool FlipFaces = true;
        public bool FlipNormals = true;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private string _targetSelectionGroup;
        private bool _flipFaces;
        private bool _flipNormals;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public FlipModel()
		{
			underlyingModifier = new Modelular.Runtime.Flip();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Flip);
			target.TargetSelectionGroup = TargetSelectionGroup;
            target.FlipFaces = FlipFaces;
            target.FlipNormals = FlipNormals;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _targetSelectionGroup != TargetSelectionGroup ||
                 _flipFaces != FlipFaces ||
                 _flipNormals != FlipNormals ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _targetSelectionGroup = TargetSelectionGroup;
             _flipFaces = FlipFaces;
             _flipNormals = FlipNormals;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
