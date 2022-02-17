Shader "Custom/Lambert_Blinnphong"
{
    Properties
    {
        _DiffuseCol("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("NormalMap", 2D) = "bump"{}
        _NormalPower("Normal Power", float) = 1.0

        _SpecCol("Specular Color", Color) = (1,1,1,1)
        _SpecPower("Specular Power", Range(10,200)) = 100
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

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
           
            //�븻 ����
            float3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal = float3(n.x * _NormalPower, n.y * _NormalPower, n.z);

            o.Alpha = c.a;
        }

        float4 LightingLambertBlinnphong(SurfaceOutput s, float3 lightDir, float viewDir, float atten)
        {
            //Diffuse����, Lambert���Ļ��
            float3 Diffuse;
            float ndotl = saturate(dot(s.Normal, lightDir));//0~1�������� ����� ����� ����
            Diffuse = ndotl * s.Albedo * _LightColor0.rgb * atten * _DiffuseCol.rgb;

            //Specular����, Blinnphong���Ļ��
            float3 Specular;
            float3 H = normalize(lightDir + viewDir);
            float spec = saturate(dot(H, s.Normal));
            spec = pow(spec, _SpecPower);//Specular ũ������
            Specular = spec * _SpecCol.rgb;//Specular ��

            float4 final;//���������� return�� ��
            final.rgb = Diffuse.rgb + Specular.rgb;
            final.a = s.Alpha;
            return final;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
