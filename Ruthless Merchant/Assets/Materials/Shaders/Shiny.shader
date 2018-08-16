// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.8039216,fgcg:0.8,fgcb:0.7921569,fgca:1,fgde:0.09,fgrn:50,fgrf:100,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:35089,y:32444,varname:node_9361,prsc:2|emission-7797-OUT,custl-9993-OUT;n:type:ShaderForge.SFN_NormalVector,id:4205,x:32872,y:32594,prsc:2,pt:False;n:type:ShaderForge.SFN_LightVector,id:2815,x:32872,y:32464,varname:node_2815,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:8116,x:33494,y:33028,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_8116,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_HalfVector,id:6362,x:32884,y:32326,varname:node_6362,prsc:2;n:type:ShaderForge.SFN_Dot,id:3741,x:33162,y:32327,varname:node_3741,prsc:2,dt:0|A-6362-OUT,B-4205-OUT;n:type:ShaderForge.SFN_Dot,id:7681,x:33074,y:32537,varname:node_7681,prsc:2,dt:0|A-2815-OUT,B-4205-OUT;n:type:ShaderForge.SFN_Fresnel,id:7361,x:33514,y:32350,varname:node_7361,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6251,x:33688,y:32374,varname:node_6251,prsc:2|A-7361-OUT,B-5508-OUT;n:type:ShaderForge.SFN_Vector1,id:5508,x:33514,y:32487,varname:node_5508,prsc:2,v1:0.3;n:type:ShaderForge.SFN_Multiply,id:9993,x:33767,y:32664,varname:node_9993,prsc:2|A-7948-OUT,B-7633-RGB,C-8605-OUT,D-8116-RGB,E-983-RGB;n:type:ShaderForge.SFN_LightAttenuation,id:8605,x:33494,y:32856,varname:node_8605,prsc:2;n:type:ShaderForge.SFN_LightColor,id:7633,x:33494,y:32719,varname:node_7633,prsc:2;n:type:ShaderForge.SFN_Fresnel,id:518,x:32989,y:32089,varname:node_518,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:7124,x:33174,y:32089,varname:node_7124,prsc:2|IN-518-OUT;n:type:ShaderForge.SFN_Power,id:1422,x:33514,y:32203,varname:node_1422,prsc:2|VAL-7124-OUT,EXP-1678-OUT;n:type:ShaderForge.SFN_Vector1,id:1678,x:33162,y:32223,varname:node_1678,prsc:2,v1:8;n:type:ShaderForge.SFN_Add,id:6646,x:33869,y:32288,varname:node_6646,prsc:2|A-3741-OUT,B-1422-OUT,C-6251-OUT;n:type:ShaderForge.SFN_Divide,id:7141,x:34031,y:32395,varname:node_7141,prsc:2|A-3741-OUT,B-3062-OUT;n:type:ShaderForge.SFN_Vector1,id:3062,x:33845,y:32463,varname:node_3062,prsc:2,v1:3;n:type:ShaderForge.SFN_Step,id:2498,x:33327,y:32630,varname:node_2498,prsc:2|A-8251-OUT,B-7681-OUT;n:type:ShaderForge.SFN_Vector1,id:8251,x:33145,y:32707,varname:node_8251,prsc:2,v1:0.25;n:type:ShaderForge.SFN_Step,id:7213,x:34213,y:32445,varname:node_7213,prsc:2|A-7009-OUT,B-7141-OUT;n:type:ShaderForge.SFN_Vector1,id:7009,x:34031,y:32533,varname:node_7009,prsc:2,v1:0.25;n:type:ShaderForge.SFN_Slider,id:3339,x:33145,y:32821,ptovrint:False,ptlb:PosterizeEffect,ptin:_PosterizeEffect,varname:node_3339,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Lerp,id:7948,x:33497,y:32584,varname:node_7948,prsc:2|A-7681-OUT,B-2498-OUT,T-3339-OUT;n:type:ShaderForge.SFN_Lerp,id:9269,x:34396,y:32387,varname:node_9269,prsc:2|A-7141-OUT,B-7213-OUT,T-3339-OUT;n:type:ShaderForge.SFN_Color,id:983,x:33494,y:33220,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_983,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1128,x:34568,y:32484,varname:node_1128,prsc:2|A-9269-OUT,B-8116-RGB,C-983-RGB;n:type:ShaderForge.SFN_Multiply,id:7026,x:33673,y:31968,varname:node_7026,prsc:2|A-2629-OUT,B-6691-OUT;n:type:ShaderForge.SFN_Add,id:7797,x:34803,y:32402,varname:node_7797,prsc:2|A-2322-OUT,B-9945-OUT,C-1128-OUT;n:type:ShaderForge.SFN_Step,id:5351,x:33844,y:32046,varname:node_5351,prsc:2|A-2741-OUT,B-7026-OUT;n:type:ShaderForge.SFN_Vector1,id:2741,x:33654,y:32125,varname:node_2741,prsc:2,v1:0.25;n:type:ShaderForge.SFN_Lerp,id:1694,x:34031,y:32005,varname:node_1694,prsc:2|A-7026-OUT,B-5351-OUT,T-3339-OUT;n:type:ShaderForge.SFN_Multiply,id:2322,x:34511,y:32060,varname:node_2322,prsc:2|A-6936-RGB,B-8966-OUT,C-1694-OUT;n:type:ShaderForge.SFN_Slider,id:8966,x:34067,y:31920,ptovrint:False,ptlb:Shiny,ptin:_Shiny,varname:node_8966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3675214,max:1;n:type:ShaderForge.SFN_Color,id:6936,x:34236,y:31748,ptovrint:False,ptlb:ShineColor,ptin:_ShineColor,varname:node_6936,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:6691,x:33449,y:32076,ptovrint:False,ptlb:ShineSize,ptin:_ShineSize,varname:node_6691,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:2629,x:33428,y:31927,varname:node_2629,prsc:2|VAL-3741-OUT,EXP-9122-OUT;n:type:ShaderForge.SFN_Vector1,id:9122,x:33187,y:32009,varname:node_9122,prsc:2,v1:6;n:type:ShaderForge.SFN_AmbientLight,id:3122,x:34210,y:32123,varname:node_3122,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:1784,x:34210,y:32273,ptovrint:False,ptlb:AmbientLight,ptin:_AmbientLight,varname:node_1784,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:9945,x:34409,y:32239,varname:node_9945,prsc:2|A-3122-RGB,B-1784-OUT,C-8116-RGB,D-983-RGB;proporder:3339-8116-983-8966-6936-6691-1784;pass:END;sub:END;*/

Shader "Shader Forge/Shiny" {
    Properties {
        _PosterizeEffect ("PosterizeEffect", Range(0, 1)) = 0
        _Texture ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Shiny ("Shiny", Range(0, 1)) = 0.3675214
        _ShineColor ("ShineColor", Color) = (1,1,1,1)
        _ShineSize ("ShineSize", Float ) = 1
        _AmbientLight ("AmbientLight", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _PosterizeEffect;
            uniform float4 _Color;
            uniform float _Shiny;
            uniform float4 _ShineColor;
            uniform float _ShineSize;
            uniform float _AmbientLight;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
////// Emissive:
                float node_3741 = dot(halfDirection,i.normalDir);
                float node_7026 = (pow(node_3741,6.0)*_ShineSize);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float node_7141 = (node_3741/3.0);
                float3 emissive = ((_ShineColor.rgb*_Shiny*lerp(node_7026,step(0.25,node_7026),_PosterizeEffect))+(UNITY_LIGHTMODEL_AMBIENT.rgb*_AmbientLight*_Texture_var.rgb*_Color.rgb)+(lerp(node_7141,step(0.25,node_7141),_PosterizeEffect)*_Texture_var.rgb*_Color.rgb));
                float node_7681 = dot(lightDirection,i.normalDir);
                float3 finalColor = emissive + (lerp(node_7681,step(0.25,node_7681),_PosterizeEffect)*_LightColor0.rgb*attenuation*_Texture_var.rgb*_Color.rgb);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _PosterizeEffect;
            uniform float4 _Color;
            uniform float _Shiny;
            uniform float4 _ShineColor;
            uniform float _ShineSize;
            uniform float _AmbientLight;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float node_7681 = dot(lightDirection,i.normalDir);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 finalColor = (lerp(node_7681,step(0.25,node_7681),_PosterizeEffect)*_LightColor0.rgb*attenuation*_Texture_var.rgb*_Color.rgb);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
