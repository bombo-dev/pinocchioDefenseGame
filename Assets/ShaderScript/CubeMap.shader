Shader "Custom/CubeMap"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("NormalMap", 2D) = "bump"{}
        _CubeMap("CubeMap", cube) = ""{}
        _CubeMapPower("CubeMapPower",Range(0,20)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex, _BumpMap;
        samplerCUBE _CubeMap;
        float _CubeMapPower;

        struct Input
        {
            float2 uv_MainTex,uv_BumpMap;
            float3 worldRefl;
            INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
           
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Emission = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal)) * _CubeMapPower;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
