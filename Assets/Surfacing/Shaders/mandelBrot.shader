Shader "Unlit/mandelBrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ZReal("Z real", Range(-10, 10)) = 0
		_ZImg("Z imaginary", Range(-10, 10)) = 0
		_Iter("iterations", Range(0, 10000)) = 0
		_zoom("Zoom", Range(1, 5)) = 1
		_moveX("X move", Range(-2, 2)) = 0
		_moveY("Y move", Range(-2, 2)) = 0
		_rad("radius", Range(0, 10)) = 4
		//_CReal ("C real", Range[-1, 1]) = 0
		//_CImg ("C imaginary", Range[-1, 1]) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
     
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _ZReal;
			float _ZImg;
			float _Iter;
			float _zoom;
			float _moveX;
			float _moveY;
			float _rad;

			int mandelbrot(float cA, float cB) {

				float zA = _ZReal;
				float zB = _ZImg;
				int Count = 0;
				int iterations = (int)_Iter;

				for (int i = 0; i < iterations; i++) {

					Count++;
					float aa = zA * zA;
					float bb = zB * zB;
					float A = aa - bb + cA;
					float B = (2 * zA * zB) + cB;
					float dist = (A * A) + (B * B);
					if (dist > _rad) {
						return Count;
					}
					zA = A;
					zB = B;
				}

				return -1;


			}


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float cA = ((i.uv.x - 0.5) * _zoom) + _moveX;
				float cB = ((i.uv.y - 0.5) * _zoom) + _moveY;
                // sample the texture
				int countOut = mandelbrot(cA, cB);
				float colCount = (float)countOut / _Iter;


				float alpha = 1;
				if (countOut == -1){
					colCount = 0;
					alpha = 0;
				}
					

                fixed4 col = float4(colCount, colCount, colCount, alpha);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
