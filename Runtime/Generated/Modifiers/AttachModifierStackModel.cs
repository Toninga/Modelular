using UnityEngine;

namespace Modelular.Runtime
{
	public class AttachModifierStackModel : ModifierModel
	{
		#region Fields

		public ModularMesh LinkedMesh;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private ModularMesh _linkedMesh;
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
			target.LinkedMesh = LinkedMesh;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _linkedMesh != LinkedMesh ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _linkedMesh = LinkedMesh;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
