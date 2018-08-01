Shader "Geometry Shader/target 4.6 (gl4.1)"
{
	SubShader 
	{
        Pass 
		{
            CGPROGRAM

            #pragma vertex vert
			#pragma geometry geom
            #pragma fragment frag
			#pragma target gl4.1

			struct v2f_surf 
			{
				float4 pos : SV_POSITION;
		    };

            v2f_surf vert(float4 v:POSITION)
			{
                 v2f_surf o;
				 UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
				 o.pos = UnityObjectToClipPos (v);

				 return o;
            }

			[maxvertexcount(3)]
			void geom(triangle v2f_surf input[3], inout TriangleStream<v2f_surf> triStream)
			{
				triStream.Append( input[0] );
				triStream.Append( input[1] );
				triStream.Append( input[2] );

				triStream.RestartStrip();
			}

            fixed4 frag() : SV_Target 
			{
                return fixed4(0.0, 1.0, 0.0, 1.0);
            }

            ENDCG
        }
    }

	FallBack "Geometry Shader/Fallback"
}
