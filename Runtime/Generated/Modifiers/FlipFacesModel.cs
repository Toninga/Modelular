using UnityEngine;

namespace Modelular.Runtime
{
	public class FlipFacesModel : ModifierModel
	{
		#region Fields

		public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public FlipFacesModel()
		{
			underlyingModifier = new Modelular.Runtime.FlipFaces();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.FlipFaces);
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
