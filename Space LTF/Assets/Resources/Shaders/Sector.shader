// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Sector" 
{

	Properties{
		_MainTex("Albedo Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Val("Height value", Range(-1, 1)) = 0
		_ObjectPos("Object Pos", Vector) = (0, 0, 0, 0)
		_LookDirection("Look Direction", Vector) = (0, 0,0,0)
	}

		SubShader{
		Tags{ "Queue" = "Transparent"  "RenderType" = "Transparent" }

		ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha


		Pass{

				CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

				struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
			};

			float4 _MainTex_ST;
			fixed2 _LookDirection;
			fixed4 _ObjectPos;
			float _Val;
			sampler2D _MainTex;
			float4 _TintColor;


			bool IsAngLessNormazied(fixed2 a, fixed2 b, float cos)
			{
				float aa = a.x * b.x + a.y * b.y;
				return aa > cos;
			}

			fixed2 NormalizeFastSelf(fixed2 v)
			{
				float d = sqrt(v.x * v.x + v.y * v.y);
				v.x = v.x / d;
				v.y = v.y / d;
				return v;
			}
			

			v2f vert(appdata v)
			{
				v2f o;
				o.uv = v.uv;// TRANSFORM_TEX(v.uv, _MainTex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target{
				float xx = i.worldPos.x - _ObjectPos.x;
				float zz = i.worldPos.z - _ObjectPos.z;

				fixed2 f1 = NormalizeFastSelf(fixed2(xx, zz));

				if (!IsAngLessNormazied(f1, _LookDirection, _Val)) 
				{
					discard;
					//return fixed4(0, 0, 0, 0);
				}
				fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
				return col;
			}


		ENDCG
	}
		

	}
}