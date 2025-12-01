using UnityEngine;

namespace Modelular.Runtime
{
	public class FlipFacesModel : ModifierModel
	{
		#region Fields

		//[Field]

		// Replicated fields for change detection
		private bool _enabled;
		//[ReplicatedField]

		#endregion
		public FlipFacesModel()
		{
			underlyingModifier = new Modelular.Runtime.FlipFaces();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.FlipFaces);
			//[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			//[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			//[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
