// Wireframe.shader
Shader "Custom/Wireframe"
{
    Properties
    {
        [MainColor] _WireColor  ("Wire color",  Color)  = (0.2, 0.8, 0.2, 1)
        _WireWidth  ("Wire width",  Float)  = 1.5
        _WireFalloff("Wire falloff",Float)  = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            Offset -1, -1          // pull slightly toward camera to avoid z-fighting

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _WireColor;
            float  _WireWidth;
            float  _WireFalloff;

            struct appdata {
                float4 vertex : POSITION;
                float3 uv2    : TEXCOORD1; // barycentric coords in UV1
            };
            struct v2f {
                float4 pos   : SV_POSITION;
                float3 bary  : TEXCOORD0;
            };

            v2f vert(appdata v) {
                v2f o;
                o.pos  = UnityObjectToClipPos(v.vertex);
                o.bary = v.uv2;
                return o;
            }

            float edgeFactor(float3 bary) {
                float3 d = fwidth(bary);
                float3 a = smoothstep(float3(0,0,0), d * _WireWidth, bary);
                return min(min(a.x, a.y), a.z);
            }

            fixed4 frag(v2f i) : SV_Target {
                float e = edgeFactor(i.bary);
                float alpha = (1.0 - e) * _WireColor.a;
                clip(alpha - 0.01);
                return float4(_WireColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}