using Modelular.Runtime;
using UnityEngine;

[ExecuteAlways]
public class VertsController : MonoBehaviour
{
    public Modelular.Runtime.CopyToRadiusModel Copy;

    public int MaxCount = 32;
    public int Count = 32;

    private void Update()
    {
        Apply();
    }
    void Apply()
    {
        if (Copy == null)
            return;

        if (Count > MaxCount) Count = MaxCount;
        if (Count < 0) Count = 0;
        Copy.Arc01 = (float)Count / MaxCount;
        Copy.Count = Count;
    }
    private void Reset()
    {
        if (TryGetComponent(out ModularMesh comp))
        {
            foreach (var model in comp.Modifiers)
            {
                if (model is CopyToRadiusModel copy)
                {
                    Copy = copy;
                }
            }
        }
    }


    private void OnValidate() => Apply();
    private void OnAnimatorMove() => Apply();
    void OnDidApplyAnimationProperties() => Apply();
}