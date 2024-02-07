Shader "Hidden/BlindFPS/VisibilityBuffer"
{
    Properties
    {
        _OuterRadius ("OuterRadius", float) = 1.0
        _InnerRadius ("InnerRadius", float) = 1.0
        _Intensity ("Intensity", float) = 1.0
        _Center ("Center", Vector) = (0, 0, 0, 0)
        _StartFallOffDistance("Start FallOff", float) = 50.0
        _EndFallOffDistance("End FallOff", float) = 80.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        LOD 100

        Pass
        {
            Name "Visibility Buffer"
            Blend One One
            ZWrite Off
            ZTest Always
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexData
            {
                float4 Vertex : POSITION;
                float2 UV : TEXCOORD0;
            };

            struct VertexToFragment
            {
                float2 UV : TEXCOORD0;
                float4 Vertex : SV_POSITION;
            };

            VertexToFragment vert(VertexData v)
            {
                VertexToFragment o;
                o.Vertex = v.Vertex;
                o.UV = v.UV;
                return o;
            }

            float _OuterRadius;
            float _InnerRadius;
            float _Intensity;
            float4 _Center;

            float _StartFallOffDistance;
            float _EndFallOffDistance;

            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(0); // position
            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(1); // depth

            float remap_value(float value, float from1, float to1, float from2, float to2)
            {
                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }

            fixed4 frag(VertexToFragment i) : SV_Target
            {
                float4 worldPositionBuffer = UNITY_READ_FRAMEBUFFER_INPUT(0, i.UV);

                float3 worldPosition = worldPositionBuffer.xyz;
                float distanceToCenter = distance(_Center.xyz, worldPosition);

                float depth = UNITY_READ_FRAMEBUFFER_INPUT(1, i.UV).r;

                fixed4 col = fixed4(0, 0, 0, 0);

                if (distanceToCenter <= _OuterRadius && distanceToCenter >= _InnerRadius && depth > 0.0)
                {
                    float distanceToCamera = distance(_WorldSpaceCameraPos, _Center.xyz);
                    float strength = 1 - lerp(0, 1, distanceToCenter / _OuterRadius);

                    float falloffT = saturate(remap_value(distanceToCamera, _StartFallOffDistance, _EndFallOffDistance, 0.0, 1.0));

                    col = fixed4(strength, strength, strength, 1.0) * _Intensity * (1 - falloffT);
                }

                return col;
            }
            ENDCG
        }
    }
}