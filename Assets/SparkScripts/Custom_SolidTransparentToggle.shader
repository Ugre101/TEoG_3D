Shader "Custom/SolidTransparentToggle"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Toggle("Toggle", Range(0, 1)) = 0
        _MainTex("Texture", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float toggle : TEXCOORD1;
            };

            float4 _Color;
            float _Toggle;
            sampler2D _MainTex;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.toggle = _Toggle;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                col.a = i.toggle * tex2D(_MainTex, i.uv).a;
                col.rgb *= col.a;
                clip(col.a - _Cutoff);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}