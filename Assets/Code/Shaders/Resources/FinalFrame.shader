Shader "Hidden/BlindFPS/FinalFrame"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.1, 0.1, 0.1, 1.0)
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
            name "Final Pass"
            
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

            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(0); // albedo
            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(1); // visibility
            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(2); // normals 
            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(3); // position
            UNITY_DECLARE_FRAMEBUFFER_INPUT_FLOAT(4); // depth 

            float4 _BaseColor;

            fixed4 frag(VertexToFragment i) : SV_Target
            {
                float4 albedo = UNITY_READ_FRAMEBUFFER_INPUT(0, i.UV); 
                float4 visibility = UNITY_READ_FRAMEBUFFER_INPUT(1, i.UV);
                float4 normals = UNITY_READ_FRAMEBUFFER_INPUT(2, i.UV);
                float4 worldPosition = UNITY_READ_FRAMEBUFFER_INPUT(3, i.UV);

                float depth = UNITY_READ_FRAMEBUFFER_INPUT(4, i.UV).r;

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - worldPosition.xyz);

                float fresnel = pow(1 - saturate(dot(normals.xyz, viewDirection)), 2);

                float3 fresnelColor = albedo.xyz * fresnel;

                float3 surfaceColor = _BaseColor;
                float3 finalColor = surfaceColor.xyz * saturate(visibility.xyz);

                //finalColor = float3(depth, depth, depth);
                
                fixed4 col = fixed4(finalColor.xyz, 1.0);
                return col;
            }
            ENDCG
        }
    }
}