using UnityEngine;

namespace Modelular.Runtime
{
	public class FlipNormalsModel : ModifierModel
	{
		#region Fields

		public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public FlipNormalsModel()
		{
			underlyingModifier = new Modelular.Runtime.FlipNormals();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.FlipNormals);
			target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
