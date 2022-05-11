Shader "Custom/Lambert_Blinnphong"
{
    Properties
    {
        _DiffuseCol("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("NormalMap", 2D) = "bump"{}
        _NormalPower("Normal Power", float) = 1.0

         _Emission("Emission (RGB)", Color) = (0,0,0,1)

        _SpecCol("Specular Color", Color) = (1,1,1,1)
        _SpecPower("Specular Power", Range(10,200)) = 100

        [Header(doublePassOutline)]
        _OutLineOption("OutLineOption", int) = 0 //0. NoOutLine       1.Black      2.Color 
        _OutLineColor("OutLineColor", Color) = (0,0,0,1)
        _OutLinePower("OutLine Power", float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf LambertBlinnphong

        sampler2D _MainTex;
        sampler2D _BumpMap;

        float4 _DiffuseCol;
        float4 _SpecCol;
        float _NormalPower;
        float _SpecPower;

        //OutLine
        float _OutLinePower;
        float4 _OutLineColor;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        //스크립트로 수정할 프로퍼티(Instancing Buffer)
        UNITY_INSTANCING_BUFFER_START(Props)

            //Emission
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Emission)
            //OutLineOption
            UNITY_DEFINE_INSTANCED_PROP(int, _OutLineOption)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 m = tex2D (_MainTex, IN.uv_MainTex);
            float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            o.Normal = float3(n.x * _NormalPower, n.y * _NormalPower, n.z);
            o.Albedo = m.rgb;
            o.Alpha = m.a;
            o.Emission = UNITY_ACCESS_INSTANCED_PROP(Props, _Emission);

        }

        float4 LightingLambertBlinnphong(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            //Diffuse구현, Lambert공식사용
            
            float3 Diffuse;
            float ndotl = saturate(dot(s.Normal, lightDir));//0~1범위에서 벗어나는 결과값 조정
            Diffuse = ndotl * s.Albedo * _LightColor0.rgb * atten * _DiffuseCol.rgb;

            //Specular구현, Blinnphong공식사용
            float3 Specular;
            float3 H = normalize(lightDir + viewDir);
            float spec = saturate(dot(H, s.Normal));
            spec = pow(spec, _SpecPower);//Specular 크기조절
            Specular = spec * _SpecCol.rgb;//Specular 색

            float4 final;//최종적으로 return할 값

            //Fresnel 외곽선
            
            float rim = abs(dot(s.Normal, viewDir));


            if (rim > _OutLinePower || UNITY_ACCESS_INSTANCED_PROP(Props, _OutLineOption) == 0)
            {
                final.rgb = Diffuse.rgb + Specular.rgb;
            }
            else
            {
                if (UNITY_ACCESS_INSTANCED_PROP(Props, _OutLineOption) == 1)
                {
                    final.rgb = -1;
                }
                else if (UNITY_ACCESS_INSTANCED_PROP(Props, _OutLineOption) == 2)
                {
                    final.rgb = _OutLineColor;
                }
            }
            final.a = s.Alpha;
            return final;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
