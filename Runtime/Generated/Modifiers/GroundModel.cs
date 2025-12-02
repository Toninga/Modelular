using UnityEngine;

namespace Modelular.Runtime
{
	public class GroundModel : ModifierModel
	{
		#region Fields

		public Color Color = Color.white;
        public string OutputSelectionGroup;
        public Vector2 Size = Vector2.one * 100f;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Color _color;
        private string _outputSelectionGroup;
        private Vector2 _size;
        //[ReplicatedField]

		#endregion
		public GroundModel()
		{
			underlyingModifier = new Modelular.Runtime.Ground();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Ground);
			target.Color = Color;
            target.OutputSelectionGroup = OutputSelectionGroup;
            target.Size = Size;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _color != Color ||
                 _outputSelectionGroup != OutputSelectionGroup ||
                 _size != Size ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _color = Color;
             _outputSelectionGroup = OutputSelectionGroup;
             _size = Size;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
