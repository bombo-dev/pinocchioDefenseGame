Shader "Custom/NightToon"
{
    Properties
    {
        [Header(Base)]
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _BumpTex("Normal Textrue", 2D) = "bump" {}
        _NormalPower("Normal Power", float) = 1.0
        _lampColor("Color (RGB)", Color) = (1,1,1,1)
        _Tiling("Tiling",Range(0,20)) = 1.0

        [Header(ToonStep)]
        _Step("Step", Range(0,1)) = 1
        _StepOffset("_StepOffset",Range(-1,1)) = 0
        [IntRange]_StepAmount("_StepAmount", Range(1,16)) = 2

        [Header(Specular)]
        _SpecularSize("SpecularSize", float) = 30
        _SpecularColor("Specular Color", Color) = (1,1,1,1)

    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Toon fullforwardshadows

        sampler2D _MainTex;
        sampler2D _BumpTex;
        float _NormalPower;
        float _Tiling;
        float4 _lampColor;

        //ToonStep
        float _Step;
        float _StepAmount;
        float _StepOffset;

        //Specular
        float _SpecularSize;
        float3 _SpecularColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
        };

        struct ToonSurfaceOutput
        {
            fixed3 Albedo;
            half3 Emission;
            fixed3 Specular;
            fixed Alpha;
            fixed3 Normal;
        };


        void surf(Input IN, inout ToonSurfaceOutput o)
        {
            float4 m = tex2D(_MainTex, IN.uv_MainTex * _Tiling);
            float3 n = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex * _Tiling));
            m *= _lampColor;
            o.Normal = float3(n.x * _NormalPower, n.y * _NormalPower, n.z);
            o.Albedo = m.rgb;
            o.Alpha = m.a;
        }

        float4 LightingToon(ToonSurfaceOutput s, float3 lightDir, float3 viewDir, float shadowAttenuation)
        {
            //Diffuse
            float DiffuseLight = dot(s.Normal, lightDir);//Diffuse 연산, lambert

            //Step이 작을수록 숫자가 커져 floor함수로 리턴할 정수값의 범위 증가
            DiffuseLight /= _Step;

            // floor -> 내림한 정수를 리턴한다, 계단형 그래프, 툰쉐이더의 층 생성
            float lightIntensity = floor(DiffuseLight);

            //Fresnel 외곽선
            float rim = abs(dot(s.Normal, viewDir));
            if (rim > 0.3)
            {
                rim = 1;
            }
            else
            {
                rim = -1;//최종적으로 ambient color가 더해져 밝아지기 때문에 음수값을 준다
            }

            //안티앨리어싱
            //fwidth -> abs(ddx(x)) + abs(ddy(x))  , 윈도우공간x,y에대한 편미분 절댓값 
            //-> 즉 화면 공간좌표에 대한값의 변화(픽셀차이)
            //float change = fwidth(DiffuseLight);//픽셀 변화율

            // float smoothing = smoothstep(0, change, frac(DiffuseLight));
            //lightIntensity = lightIntensity + smoothing;

            //StepAmount로 1이하의 유효한 라이팅 값들을 생성, _StepAmount클수록 유효한값 증가
            lightIntensity = (lightIntensity / _StepAmount) + _StepOffset;
            lightIntensity = saturate(lightIntensity);//0~1로 범위조정


            //Shadow
            #ifdef USING_DIRECTIONAL_LIGHT
            //for directional lights, get a hard vut in the middle of the shadow attenuation
            //그림자 감쇠 처리전 안티앨리어싱
            float attenuationChange = fwidth(shadowAttenuation) * 0.5;
            float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
            #else
            //for other light types (point, spot), put the cutoff near black, so the falloff doesn't affect the range
            float attenuationChange = fwidth(shadowAttenuation);
            float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
            #endif
            lightIntensity = lightIntensity * shadow;


            //Specular
            float3 SpecularLight;
            float3 fH = normalize(lightDir + viewDir);
            float NdotfH = pow(saturate(dot(s.Normal, fH)), _SpecularSize);
            if (NdotfH > 0.8)
            {
                NdotfH = 1;
            }
            else
            {
                NdotfH = 0;
            }
            SpecularLight = NdotfH * _SpecularColor * _LightColor0;

            float4 final;
            final.rgb = (s.Albedo.rgb * lightIntensity * _LightColor0.rgb) + SpecularLight;
            final.a = s.Alpha;
            return final;
        }
        ENDCG
        }
            FallBack "Diffuse"
}
