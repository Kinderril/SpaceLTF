Shader "Hidden/SSAOMask"
{	Properties
	{
		_StencilType("StencilTyper", int) = 3
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Fog { Mode Off }
			ZWrite Off
			ZTest Always 
			
			Stencil 
			  {
				Ref [_StencilType]
				ReadMask  3       
				Comp Equal
			  }
			  
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			float _StencilType;
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = v.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return 1;
			}
			ENDCG
		}
	}
}
