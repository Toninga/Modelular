using UnityEngine;

namespace Modelular.Runtime
{
	public class ColorGradientModel : ModifierModel
	{
		#region Fields

		public Vector3 Direction = Vector3.up;
        public Color ColorA;
        public Color ColorB = Color.white;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Vector3 _direction;
        private Color _colorA;
        private Color _colorB;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public ColorGradientModel()
		{
			underlyingModifier = new Modelular.Runtime.ColorGradient();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.ColorGradient);
			target.Direction = Direction;
            target.ColorA = ColorA;
            target.ColorB = ColorB;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _direction != Direction ||
                 _colorA != ColorA ||
                 _colorB != ColorB ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _direction = Direction;
             _colorA = ColorA;
             _colorB = ColorB;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
