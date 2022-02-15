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
            float DiffuseLight = dot(s.Normal, lightDir);//Diffuse ����, lambert

            //Step�� �������� ���ڰ� Ŀ�� floor�Լ��� ������ �������� ���� ����
            DiffuseLight /= _Step;

            // floor -> ������ ������ �����Ѵ�, ����� �׷���, �����̴��� �� ����
            float lightIntensity = floor(DiffuseLight);

            //Fresnel �ܰ���
            float rim = abs(dot(s.Normal, viewDir));
            if (rim > 0.3)
            {
                rim = 1;
            }
            else
            {
                rim = -1;//���������� ambient color�� ������ ������� ������ �������� �ش�
            }

            //��Ƽ�ٸ����
            //fwidth -> abs(ddx(x)) + abs(ddy(x))  , ���������x,y������ ��̺� ���� 
            //-> �� ȭ�� ������ǥ�� ���Ѱ��� ��ȭ(�ȼ�����)
            //float change = fwidth(DiffuseLight);//�ȼ� ��ȭ��

            // float smoothing = smoothstep(0, change, frac(DiffuseLight));
            //lightIntensity = lightIntensity + smoothing;

            //StepAmount�� 1������ ��ȿ�� ������ ������ ����, _StepAmountŬ���� ��ȿ�Ѱ� ����
            lightIntensity = (lightIntensity / _StepAmount) + _StepOffset;
            lightIntensity = saturate(lightIntensity);//0~1�� ��������


            //Shadow
            #ifdef USING_DIRECTIONAL_LIGHT
            //for directional lights, get a hard vut in the middle of the shadow attenuation
            //�׸��� ���� ó���� ��Ƽ�ٸ����
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
