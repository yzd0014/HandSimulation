Shader "Unlit/ToonTest"
{
    Properties
    {

		//use solid color map
		//use multiply trick from pepper
		//use selected grey tones

        _MainTex ("Texture", 2D) = "white" {}

		_Normal("normalMap", 2D) = "bump"{}

		_top("Top Color", Color) = (1, 1, 1, 1)
		_mid("Middle Color", Color) = (0.5, 0.5, 0.5, 1)
		_bottom("Bottom Color", Color) = (0.15, 0.15, 0.15, 1)
		_spec("Speuclar Color", Color) = (1, 1, 1, 1)

		_tThresh("Top Threshold", Range(0, 1)) = 0.5
		_mThresh("Middle Threshold", Range(0, 1)) = 0.15

		_thThresh("Top Hash Threshold", Range(0, 1)) = 0.75
		_mhThresh("Middle Hash Threshold", Range(0, 1)) = 0.25
		_thPow("Top Hash Thingy", Range(0, 10)) = 1
    }
    SubShader
    {
        Pass
        {
		Tags
	{
		"LightMode" = "ForwardBase"
		"PassFlags" = "OnlyDirectional"
	}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase 
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float3 normal : NORMAL;
            };

			sampler2D _Normal;
            sampler2D _MainTex;
            float4 _MainTex_ST;

			fixed4 _top;
			fixed4 _mid;
			fixed4 _bottom;
			fixed4 _spec;

			float _tThresh;
			float _mThresh;
			float _thThresh;
			float _mhThresh;

			float _thPow;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
				o.viewDir = WorldSpaceViewDir(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float4 multiply(float4 A, float4 B) {

				return float4(A.x *B.x, A.y * B.y, A.z * B.z, 1);

			}

            fixed4 frag (v2f i) : SV_Target
            {
				float3 norm = tex2D(_Normal, i.uv);
				float3 worldNorm = normalize(UnityObjectToWorldNormal(norm));

				float3 worldNormal = UnityObjectToWorldNormal(i.normal);


				float3 addNorm = float3(norm.x * 2 - 1, norm.y * 2 - 1, 0);
				float3 correctedNorm = float3(i.normal.x * 2 - 1, i.normal.y * 2 - 1, i.normal.z) + addNorm;
				
				correctedNorm = float3((correctedNorm.x + 1) / 2, (correctedNorm.y +1) / 2, correctedNorm.z);
				
				worldNormal = UnityObjectToWorldNormal(correctedNorm);

				
				float3 normal = normalize(worldNormal);
				
				float NdotL = saturate(dot(_WorldSpaceLightPos0, normal));

				float topMix = NdotL < _tThresh ? 0 : 1;
				float midMix = NdotL < _mThresh ? 0 : 1;


				float thRange = _mThresh - _thThresh;
				float xTrav = (NdotL - _thThresh) / thRange;
				float x = xTrav%0.2;
				float NdotV = saturate(dot(i.viewDir, normal));

				topMix = NdotL < _thThresh ? topMix * (step(0.2-x, pow(NdotV, _thPow))) : topMix;


				//over operators


				
				float4 col = (_mid*midMix + _bottom * (1 - midMix)) / (midMix + (1 - midMix));
				col = (_top*topMix + col * (1 - topMix)) / (topMix + (1 - topMix));
			
				float mix = col.r;

				col = tex2D(_MainTex, i.uv);
				float4 mixCol = lerp(col, float4(1, 1, 1, 1), mix);


				col = multiply(col, mixCol);

				float2 UV = float2(NdotL, 0);
				

				//float4 col = float4(normal , 1);
                
				
				
				// sample the texture
                //fixed4 col = tex2D(_MainTex, UV);
				//fixed4 col = float4(NdotL, 0, 0, 1);



                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
