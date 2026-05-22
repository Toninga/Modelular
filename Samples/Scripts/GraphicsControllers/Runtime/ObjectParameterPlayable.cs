using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ObjectParameterPlayable : PlayableGraphicsController
{
    public List<AnimatedParameter> AnimatedParameters = new List<AnimatedParameter>();
    protected override void Apply(float t)
    {
        foreach (var param in AnimatedParameters)
        {
            Apply(param, T);
        }
    }

    public Type[] GetSupportedTypes() => new Type[] { typeof(float), typeof(int), typeof(bool)};

    // Modify the value on the target
    private void Apply(AnimatedParameter param, float t)
    {
        if (string.IsNullOrEmpty(param.Name)) return;
        if (param.Target == null) return;
        if (Curve != null) t = param.Curve.Evaluate(t);

        // Cache the member info
        if (param.Member == null || param.Member.Name != param.Name)
        {
            param.Member = GetMemberInfoFromParameter(param);
        }
        
        if (param.Member == null)
        {
            Debug.LogWarning("No member with name '" + param.Name + "' could be found on " + param.Target);
            return;
        }

        if (param.Member is PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(float))
                {
                    propertyInfo.SetValue(param.Target, t);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(param.Target, (int)t);
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    propertyInfo.SetValue(param.Target, t > 0.5f);
                }
                else
                {
                    Debug.LogWarning("[ObjectParameterPlayable] The parameter type '" 
                        + propertyInfo.PropertyType + "' is not supported. It was ignored");
                }
            }
        }

        else if (param.Member is FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                if (fieldInfo.FieldType == typeof(float))
                {
                    fieldInfo.SetValue(param.Target, t);
                }
                else if (fieldInfo.FieldType == typeof(int))
                {
                    fieldInfo.SetValue(param.Target, (int)t);
                }
                else if (fieldInfo.FieldType == typeof(bool))
                {
                    fieldInfo.SetValue(param.Target, t > 0.5f);
                }
                else
                {
                    Debug.LogWarning("[ObjectParameterPlayable] The parameter type '"
                        + fieldInfo.FieldType + "' is not supported. It was ignored");
                }
            }
        }
    }

    public MemberInfo GetMemberInfoFromParameter(AnimatedParameter param)
    {
        Type type = param.Target.GetType();
        return type.GetMember(param.Name).FirstOrDefault();
    }

    [System.Serializable]
    public struct AnimatedParameter
    {
        public string Name;
        public UnityEngine.Object Target;
        public AnimationCurve Curve;
        public MemberInfo Member;

        public AnimatedParameter(string name, UnityEngine.Object target)
        {
            Name = name;
            Target = target;
            Curve = AnimationCurve.Linear(0,0,1,1);
            Member = null;
        }
        public AnimatedParameter(string name, UnityEngine.Object target, MemberInfo memberInfo)
        {
            Name = name;
            Target = target;
            Curve = AnimationCurve.Linear(0,0,1,1);
            Member = memberInfo;
        }
    }
}
