Shader "Custom/paralax"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormTex ("Normal Map", 2D) = "white" {}
		_BumpTex ("Height Map", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_OffsetTest ("offest tester", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _NormTex;
		sampler2D _BumpTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDirTangent;
        };

        half _Glossiness;
        half _Metallic;
		float _OffsetTest;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


		void vert(inout appdata_full v, out Input o) {

			UNITY_INITIALIZE_OUTPUT(Input, o);
			float4 objCam = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
			float3 viewDir = v.vertex.xyz - objCam.xyz;
			float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
			float3 bitTangent = cross(v.normal.xyz, v.tangent.xyz) * tangentSign;
			o.viewDirTangent = float3(dot(viewDir, v.tangent.xyz), dot(viewDir, bitTangent.xyz), dot(viewDir, v.normal.xyz));


		}
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
			//float3 normView = normalize(IN.viewDir);




			float height = (1.0 - tex2D(_BumpTex, IN.uv_MainTex).x)*0.1;

			float3 viewNorm = normalize(IN.viewDirTangent);

			float offset = dot(float3(0, 0, height), viewNorm);
			float2 newoff = -offset * viewNorm.xy;
			float2 newUV = IN.uv_MainTex + newoff;
			/*
			if (newUV.x > 1.0 || newUV.x < 0.0 || newUV.y > 1.0 || newUV.y < 0.0)
				discard;
				*/
            fixed4 c = tex2D(_MainTex, newUV) * _Color;
			//fixed4 c = float4(newoff.x, newoff.y, 0, 1);
			o.Normal = UnpackNormal(tex2D(_NormTex, newUV));
			
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
