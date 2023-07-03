Shader "Custom/URPBlendTexture"
{
    Properties
    {
        _MainTex("Texture 1", 2D) = "white" {}
        _SecondTex("Texture 2", 2D) = "white" {}
        _Blend("Blend", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}

        Pass
        {
            HLSLPROGRAM
            #include <Library/PackageCache/com.unity.render-pipelines.core@14.0.7/ShaderLibrary/Common.hlsl>

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _SecondTex;
            float _Blend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_SecondTex, i.uv);
                fixed4 col = lerp(col1, col2, _Blend);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}