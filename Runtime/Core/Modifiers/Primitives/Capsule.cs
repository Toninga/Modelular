using NUnit.Framework;
using UnityEngine;

namespace Modelular.Runtime
{
    [ModelularInterface("Primitives/Capsule", 0)]
    public class Capsule : Modifier
    {
        #region Properties



        #endregion

        
        #region Methods
        public override StackElement Bake(StackElement previousResult)
        {
            // Logic here
            
            return previousResult;
        }

        

        #endregion
    }
}