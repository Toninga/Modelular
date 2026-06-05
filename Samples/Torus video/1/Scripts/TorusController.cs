using Modelular.Runtime;
using UnityEngine;

[ExecuteAlways]
public class TorusController : MonoBehaviour
{
    public Modelular.Runtime.TorusModel Torus;

    [Range(0.0f, 1.0f)]
    public float Arc = 1f;
    public float Thickness = 0.1f;
    public bool Caps = true;


    void Apply()
    {
        if (Torus == null)
            return;

        Torus.Arc = Arc;
        Torus.Caps = Caps;
        Torus.Thickness = Thickness;
    }
    private void Reset()
    {
        if (TryGetComponent(out ModularMesh comp))
        {
            foreach(var model in comp.Modifiers)
            {
                if (model is TorusModel torus)
                {
                    Torus = torus;
                    break;
                }
            }
        }
    }


    private void OnValidate()
    {
        Apply();
    }

    private void OnAnimatorMove()
    {
        Apply();
    }

    void OnDidApplyAnimationProperties()
    {
        Apply();
    }
}
