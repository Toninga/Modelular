using UnityEngine;

namespace Modelular.Runtime
{
	public class ColorGradientModel : ModifierModel
	{
		#region Fields

		public Color ColorA;
        public Color ColorB = Color.white;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Color _colorA;
        private Color _colorB;
        //[ReplicatedField]

		#endregion
		public ColorGradientModel()
		{
			underlyingModifier = new Modelular.Runtime.ColorGradient();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.ColorGradient);
			target.ColorA = ColorA;
            target.ColorB = ColorB;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _colorA != ColorA ||
                 _colorB != ColorB ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _colorA = ColorA;
             _colorB = ColorB;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
