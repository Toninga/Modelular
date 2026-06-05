using Modelular.Runtime;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class TextController : MonoBehaviour
{
    public TMP_Text Text;

    public float TextValue = 1.0f;
    public int Decimals = 2;

    void Apply()
    {
        if (Text == null)
            return;

        var factor = Mathf.Pow(10, Decimals);
        Text.text = (Mathf.Round(TextValue * factor) / factor).ToString();
    }
    private void Reset()
    {
        Text = GetComponent<TMP_Text>();
    }


    private void OnValidate() => Apply();
    private void OnAnimatorMove() => Apply();
    void OnDidApplyAnimationProperties() => Apply();
}