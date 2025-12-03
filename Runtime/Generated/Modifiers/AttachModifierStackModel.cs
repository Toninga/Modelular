using UnityEngine;

namespace Modelular.Runtime
{
	public class AttachModifierStackModel : ModifierModel
	{
		#region Fields

		public STransform Transform = STransform.Default();
        public ModularMesh LinkedMesh;
        public string TargetSelectionGroup;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private STransform _transform;
        private ModularMesh _linkedMesh;
        private string _targetSelectionGroup;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public AttachModifierStackModel()
		{
			underlyingModifier = new Modelular.Runtime.AttachModifierStack();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.AttachModifierStack);
			target.Transform = Transform;
            target.LinkedMesh = LinkedMesh;
            target.TargetSelectionGroup = TargetSelectionGroup;
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
                 _linkedMesh != LinkedMesh ||
                 _targetSelectionGroup != TargetSelectionGroup ||
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
             _linkedMesh = LinkedMesh;
             _targetSelectionGroup = TargetSelectionGroup;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
