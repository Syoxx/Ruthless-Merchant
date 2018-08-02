// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/TerrainEngine/BillboardTree" {
	Properties {
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	}
	
	SubShader {
		Tags { "Queue" = "Transparent-100" "IgnoreProjector"="True" "RenderType"="TreeBillboard" }
		Pass {
			ColorMask rgb
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"
			#include "../Includes/AfsBillboardShadow.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				fixed3 color : COLOR0;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			// AFS Billboard Shadow Color
			fixed4 _AfsAmbientBillboardLight;
			// AFS Tree Color
			fixed4 _AfsTreeColor;

			v2f vert (appdata_tree_billboard v) {
				v2f o;
				AfsTerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv.x = v.texcoord.x;
				o.uv.y = v.texcoord.y > 0;

				float factor = v.color.g;
				
				o.color.rgb = lerp(_AfsTreeColor.rgb, 1.0, factor);
				o.color.rgb *= lerp( ( _AfsAmbientBillboardLight.rgb ), fixed3(1,1,1), v.color.a );

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 col = tex2D( _MainTex, input.uv);
				col.rgb = col.rgb * input.color.rgb;
				// shaded billboards
				// col.rgb *= lerp( ( _AfsAmbientBillboardLight.rgb ), fixed3(1,1,1), input.color.a );
				clip(col.a);
				UNITY_APPLY_FOG(input.fogCoord, col);
				return col;
			}
			ENDCG			
		}

	}
	Fallback Off
}
