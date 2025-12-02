using UnityEngine;

namespace Modelular.Runtime
{
	public class TransformModel : ModifierModel
	{
		#region Fields

		public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale = Vector3.one;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public TransformModel()
		{
			underlyingModifier = new Modelular.Runtime.Transform();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.Transform);
			target.Position = Position;
            target.Rotation = Rotation;
            target.Scale = Scale;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _position != Position ||
                 _rotation != Rotation ||
                 _scale != Scale ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _position = Position;
             _rotation = Rotation;
             _scale = Scale;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
