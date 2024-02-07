Shader "BlindFPS/Viewmodel"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
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
            Tags
            {
                "LightMode" = "BlindFPS_Viewmodel"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexData
            {
                float4 Vertex : POSITION;
                float2 UV : TEXCOORD0;
                float3 Normal : NORMAL;
            };

            struct VertexToFragment
            {
                float2 UV : TEXCOORD0;
                float4 Vertex : SV_POSITION;
                float3 WorldNormal : TEXCOORD1;
                float3 WorldPosition : TEXCOORD2;
            };

            float4 _Color;

            VertexToFragment vert(VertexData v)
            {
                VertexToFragment o;
                o.Vertex = UnityObjectToClipPos(v.Vertex);
                o.UV = v.UV;
                o.WorldNormal = mul((float3x3)unity_ObjectToWorld, float4(v.Normal, 1.0)).xyz;
                o.WorldPosition = mul(unity_ObjectToWorld, float4(v.Vertex.xyz, 1.0)).xyz;
                return o;
            }

            fixed4 frag(VertexToFragment i) : SV_Target
            {
                float distanceToCamera = distance(_WorldSpaceCameraPos, i.WorldPosition);
                float t = saturate(distanceToCamera / 1.2);
                return lerp(_Color, float4(0.0, 0.0, 0.0, 0.0), t);
            }
            ENDCG
        }

        Pass
        {
            Tags
            {
                "LightMode" = "BlindFPS_SceneView"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexData
            {
                float4 Vertex : POSITION;
                float2 UV : TEXCOORD0;
                float3 Normal : NORMAL;
            };

            struct VertexToFragment
            {
                float2 UV : TEXCOORD0;
                float4 Vertex : SV_POSITION;
                float3 Normal : TEXCOORD1;
            };

            float4 _Color;

            VertexToFragment vert(VertexData v)
            {
                VertexToFragment o;
                o.Vertex = UnityObjectToClipPos(v.Vertex);
                o.UV = v.UV;
                o.Normal = mul((float3x3)unity_ObjectToWorld, v.Normal).xyz;
                return o;
            }

            fixed4 frag(VertexToFragment i) : SV_Target
            {
                float3 lightDir = normalize(float3(-1, -1, -1));
                float lightValue = max(dot(i.Normal, -lightDir), 0.0);
                float3 lighting = float3(0.1, 0.1, 0.1) + lightValue;

                return fixed4(_Color.xyz * lighting, 1.0);
            }
            ENDCG
        }
    }
}