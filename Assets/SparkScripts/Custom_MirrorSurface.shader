Shader "Custom/MirrorSurface"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Reflectivity("Reflectivity", Range(0,1)) = 1
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Reflectivity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y = 1 - uv.y; // flip texture vertically
                float4 tex = tex2D(_MainTex, uv);
                float4 refl = tex2D(_MainTex, reflect(uv, float2(0,1))); // reflect texture vertically
                return lerp(tex, refl, _Reflectivity);
            }
            ENDCG
        }
    }
}