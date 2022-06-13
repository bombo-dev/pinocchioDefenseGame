// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Custom/MapTest1"
{
    Properties{
        _TintColor("TintColor", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
    _LightMap("Lightmap (RGB)", 2D) = "black" {}
    _LightIntensity("Light Intensity", Range(0,2)) = 1
    }
    SubShader{ 
        Tags { "RenderType" = "Opaque" }

    Lighting off

        LOD 200

        CGPROGRAM
        #pragma surface surf LambertBlinnphong

        fixed4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
    { 
        return fixed4(s.Albedo,1);
    }

    sampler2D _MainTex;
    sampler2D _LightMap;
    float _LightIntensity;
    fixed4 _TintColor;

    struct Input
    {
        float2 uv_MainTex;
        float2 uv2_LightMap;
    };

    void surf(Input IN, inout SurfaceOutput o)
    {
        half4 c = tex2D(_MainTex, IN.uv2_LightMap);
        half4 lm = tex2D(_LightMap, IN.uv2_LightMap);

        o.Albedo = c.rgb * lm.rgb * _TintColor.rgb * _LightIntensity;
    }

    float4 LightingLambertBlinnphong(SurfaceOutput s, float3 lightDir, float3 viewDir)
    {
        //Fresnel 외곽선
        float rim = abs(dot(s.Normal, viewDir));
        if (rim > 0.5)
        {
            rim = 1;
        }
        else
        {
            rim = -10;//최종적으로 ambient color가 더해져 밝아지기 때문에 음수값을 준다
        }

        float4 final;
        //final.rgb = (s.Albedo.rgb * lightIntensity * _LightColor0.rgb * rim) + SpecularLight ;
        final.rgb = (s.Albedo.rgb * _LightColor0) * rim;
        final.a = s.Alpha;
        return final;
    }

    ENDCG
    }
        fallback "mobile/Diffuse"
}
