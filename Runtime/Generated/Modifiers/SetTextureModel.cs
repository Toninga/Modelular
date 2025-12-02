using UnityEngine;

namespace Modelular.Runtime
{
	public class SetTextureModel : ModifierModel
	{
		#region Fields

		public int MaterialIndex;
        public string PropertyName = "_MainTex";
        public Texture2D Texture;
        public bool IgnoreVertexLimits;
        //[Field]

		// Replicated fields for change detection
		private bool _enabled;
		private int _materialIndex;
        private string _propertyName;
        private Texture2D _texture;
        private bool _ignoreVertexLimits;
        //[ReplicatedField]

		#endregion
		public SetTextureModel()
		{
			underlyingModifier = new Modelular.Runtime.SetTexture();
		}
		public override void ApplyParameters()
		{
			var target = (underlyingModifier as Modelular.Runtime.SetTexture);
			target.MaterialIndex = MaterialIndex;
            target.PropertyName = PropertyName;
            target.Texture = Texture;
            target.IgnoreVertexLimits = IgnoreVertexLimits;
            //[SetProperty]
		}
		public override bool DetectChanges()
		{
			// Insert here the comparison for all properties that should be change-checked
			if
			(
			enabled != _enabled ||
			 _materialIndex != MaterialIndex ||
                 _propertyName != PropertyName ||
                 _texture != Texture ||
                 _ignoreVertexLimits != IgnoreVertexLimits ||
                //[ChangeCheck]
			false
			)
			{
				hasChanged = true;
			}
			// Reset the mirrored fields
			_enabled = enabled;
			 _materialIndex = MaterialIndex;
             _propertyName = PropertyName;
             _texture = Texture;
             _ignoreVertexLimits = IgnoreVertexLimits;
            //[ReplicatedFieldReset]

			return hasChanged;
		}
	}
}
