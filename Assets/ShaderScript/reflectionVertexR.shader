Shader "Custom/reflectionVertexR"
{
   Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _BumpTex("Normal Textrue", 2D) = "bump" {}

        _CubeBump("Normal Textrue",2D) = "bump"{}
        _CubeBumpPower("Normal Power", float) = 1.0
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
         #pragma surface surf Standard 
        #pragma target 3.0


        sampler2D _MainTex;
        sampler2D _BumpTex;

        sampler2D _CubeBump;

        float _CubeBumpPower;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
            
            float2 uv_CubeBump;
            INTERNAL_DATA//worldRefl사용시 노말맵 쓸경우 WorldReflectionVector사용
                         //픽셀당 반사벡터를 사용하기위해 탄젠트 좌표계를 월드좌표계로 
                         //변환시켜주기 위해 INTERNAL_DATA를 통하여 변환

            float3 worldRefl;

            float4 color:COLOR;///:COLOR로 태그달아줌
        };


        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);//일반텍스처
            fixed3 c_n = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));//일반텍스처 노말맵

            fixed3 d_n = UnpackNormal(tex2D(_CubeBump, IN.uv_CubeBump));//큐브맵 노말
            fixed3 wr = WorldReflectionVector(IN, o.Normal);

            fixed3 r = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, wr) * unity_SpecCube0_HDR.r;//Reflection Probe, 주변 static오브젝트 캡처해 큐브맵 저장

            o.Emission = lerp(float3(0, 0, 0), r, IN.color.r);//텍스처적용
            o.Albedo = lerp(c.rgb, float3(0, 0, 0), IN.color.r);//텍스처 적용
            o.Normal = lerp(c_n.rgb, float3(d_n.x * _CubeBumpPower,d_n.y * _CubeBumpPower, d_n.z), IN.color.r);//노말 적용


            
            o.Alpha = c.a;

        }
        ENDCG
    }
        FallBack "Diffuse"
}
