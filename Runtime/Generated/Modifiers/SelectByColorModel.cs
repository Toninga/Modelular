using UnityEngine;

namespace Modelular.Runtime
{
	public class SelectByColorModel : ModifierModel
	{
		#region Fields

		public SelectorParameters OutputParameters;
        public Color Color = Color.white;
        public float Tolerance01;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private SelectorParameters _outputParameters;
        private Color _color;
        private float _tolerance01;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public SelectByColorModel()
		{
			underlyingModifier = new Modelular.Runtime.SelectByColor();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.SelectByColor);
			target.OutputParameters = OutputParameters;
            target.Color = Color;
            target.Tolerance01 = Tolerance01;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _outputParameters != OutputParameters ||
                 _color != Color ||
                 _tolerance01 != Tolerance01 ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _outputParameters = OutputParameters;
             _color = Color;
             _tolerance01 = Tolerance01;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
