Shader "Custom/Fire"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _FireTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _FireTex;
        sampler2D _NoiseTex;

        struct Input
        {
            float2 uv_FireTex;
            float2 uv_NoiseTex;
        };

        fixed4 _Color;


        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 d = tex2D(_NoiseTex, float2(IN.uv_NoiseTex.x, IN.uv_NoiseTex.y - _Time.y)) * _Color;
            fixed4 c = tex2D (_FireTex, IN.uv_FireTex + d.r) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
