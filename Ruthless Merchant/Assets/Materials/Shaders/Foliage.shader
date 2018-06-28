// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34513,y:32708,varname:node_9361,prsc:2|emission-3113-OUT,custl-6946-OUT,clip-4191-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:33559,y:33172,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:33559,y:33046,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:33201,y:32918,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_Dot,id:7782,x:33364,y:32988,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-3925-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:32835,y:32440,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1941,x:33559,y:32829,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-544-OUT,B-7782-OUT;n:type:ShaderForge.SFN_Color,id:5927,x:32835,y:32263,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5927,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:33061,y:32379,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB;n:type:ShaderForge.SFN_Add,id:1746,x:34153,y:33214,varname:node_1746,prsc:2|A-3021-RGB,B-587-OUT,C-9103-OUT;n:type:ShaderForge.SFN_Tex2d,id:3021,x:33977,y:33283,ptovrint:False,ptlb:Alpha-Noise,ptin:_AlphaNoise,varname:node_9927,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7067-UVOUT;n:type:ShaderForge.SFN_Max,id:587,x:33977,y:33449,varname:node_587,prsc:2|A-949-OUT,B-9358-OUT;n:type:ShaderForge.SFN_ScreenPos,id:803,x:33813,y:33283,varname:node_803,prsc:2,sctp:1;n:type:ShaderForge.SFN_ValueProperty,id:9103,x:33977,y:33601,ptovrint:False,ptlb:AlphaAdder,ptin:_AlphaAdder,varname:node_9406,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Min,id:9358,x:33813,y:33500,varname:node_9358,prsc:2|A-8026-OUT,B-180-OUT;n:type:ShaderForge.SFN_Vector1,id:949,x:33813,y:33432,varname:node_949,prsc:2,v1:0;n:type:ShaderForge.SFN_OneMinus,id:8026,x:33653,y:33454,varname:node_8026,prsc:2|IN-871-OUT;n:type:ShaderForge.SFN_Vector1,id:180,x:33653,y:33584,varname:node_180,prsc:2,v1:1;n:type:ShaderForge.SFN_Power,id:871,x:33481,y:33454,varname:node_871,prsc:2|VAL-2164-OUT,EXP-8929-OUT;n:type:ShaderForge.SFN_Fresnel,id:2164,x:33310,y:33454,varname:node_2164,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8929,x:33310,y:33610,ptovrint:False,ptlb:AlphaPower,ptin:_AlphaPower,varname:_AlphaAdder_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Tex2d,id:7352,x:34153,y:33049,ptovrint:False,ptlb:Alpha-Texture,ptin:_AlphaTexture,varname:node_2957,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4191,x:34337,y:33030,varname:node_4191,prsc:2|A-7352-R,B-1746-OUT;n:type:ShaderForge.SFN_Posterize,id:1686,x:34153,y:32795,varname:node_1686,prsc:2|IN-1596-OUT,STPS-6200-OUT;n:type:ShaderForge.SFN_Lerp,id:6946,x:34337,y:32901,varname:node_6946,prsc:2|A-1686-OUT,B-3747-OUT,T-639-OUT;n:type:ShaderForge.SFN_Slider,id:639,x:33870,y:32976,ptovrint:False,ptlb:Posterize-Effect,ptin:_PosterizeEffect,varname:node_639,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Min,id:1596,x:33948,y:32712,varname:node_1596,prsc:2|A-7981-OUT,B-3747-OUT;n:type:ShaderForge.SFN_Vector1,id:7981,x:33748,y:32829,varname:node_7981,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2945,x:33559,y:32988,ptovrint:False,ptlb:Posterize-Multiplier,ptin:_PosterizeMultiplier,varname:_noiseAdder_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3747,x:33748,y:32900,cmnt:Attenuate and Color,varname:node_3747,prsc:2|A-1941-OUT,B-2945-OUT,C-3406-RGB,D-8068-OUT;n:type:ShaderForge.SFN_AmbientLight,id:2701,x:33061,y:32243,varname:node_2701,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3113,x:33392,y:32478,cmnt:Diffuse Color,varname:node_3113,prsc:2|A-1739-OUT,B-2701-RGB,C-544-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1739,x:33061,y:32175,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:node_1739,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_TexCoord,id:7067,x:33813,y:33133,varname:node_7067,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_FragmentPosition,id:629,x:32837,y:32977,varname:node_629,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:1873,x:32837,y:33111,varname:node_1873,prsc:2;n:type:ShaderForge.SFN_Subtract,id:8097,x:33031,y:33049,varname:node_8097,prsc:2|A-629-XYZ,B-1873-XYZ;n:type:ShaderForge.SFN_Normalize,id:3925,x:33201,y:33049,varname:node_3925,prsc:2|IN-8097-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6200,x:33948,y:32865,ptovrint:False,ptlb:Posterize-Number,ptin:_PosterizeNumber,varname:node_6200,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:6;proporder:851-5927-1739-7352-639-6200-2945-3021-9103-8929;pass:END;sub:END;*/

Shader "Shader Forge/Foliage" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 1
        _AlphaTexture ("Alpha-Texture", 2D) = "white" {}
        _PosterizeEffect ("Posterize-Effect", Range(0, 1)) = 1
        _PosterizeNumber ("Posterize-Number", Float ) = 6
        _PosterizeMultiplier ("Posterize-Multiplier", Float ) = 1
        _AlphaNoise ("Alpha-Noise", 2D) = "white" {}
        _AlphaAdder ("AlphaAdder", Float ) = 0
        _AlphaPower ("AlphaPower", Float ) = 2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
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
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform sampler2D _AlphaNoise; uniform float4 _AlphaNoise_ST;
            uniform float _AlphaAdder;
            uniform float _AlphaPower;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _PosterizeEffect;
            uniform float _PosterizeMultiplier;
            uniform float _AmbientMultiplier;
            uniform float _PosterizeNumber;
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
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                float4 _AlphaNoise_var = tex2D(_AlphaNoise,TRANSFORM_TEX(i.uv0, _AlphaNoise));
                clip((_AlphaTexture_var.r*(_AlphaNoise_var.rgb+max(0.0,min((1.0 - pow((1.0-max(0,dot(normalDirection, viewDirection))),_AlphaPower)),1.0))+_AlphaAdder)) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 emissive = (_AmbientMultiplier*UNITY_LIGHTMODEL_AMBIENT.rgb*node_544);
                float3 node_3747 = ((node_544*max(0,dot(lightDirection,normalize((i.posWorld.rgb-objPos.rgb)))))*_PosterizeMultiplier*_LightColor0.rgb*attenuation); // Attenuate and Color
                float3 finalColor = emissive + lerp(floor(min(1.0,node_3747) * _PosterizeNumber) / (_PosterizeNumber - 1),node_3747,_PosterizeEffect);
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
            Cull Off
            
            
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
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform sampler2D _AlphaNoise; uniform float4 _AlphaNoise_ST;
            uniform float _AlphaAdder;
            uniform float _AlphaPower;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _PosterizeEffect;
            uniform float _PosterizeMultiplier;
            uniform float _AmbientMultiplier;
            uniform float _PosterizeNumber;
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
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                float4 _AlphaNoise_var = tex2D(_AlphaNoise,TRANSFORM_TEX(i.uv0, _AlphaNoise));
                clip((_AlphaTexture_var.r*(_AlphaNoise_var.rgb+max(0.0,min((1.0 - pow((1.0-max(0,dot(normalDirection, viewDirection))),_AlphaPower)),1.0))+_AlphaAdder)) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 node_3747 = ((node_544*max(0,dot(lightDirection,normalize((i.posWorld.rgb-objPos.rgb)))))*_PosterizeMultiplier*_LightColor0.rgb*attenuation); // Attenuate and Color
                float3 finalColor = lerp(floor(min(1.0,node_3747) * _PosterizeNumber) / (_PosterizeNumber - 1),node_3747,_PosterizeEffect);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _AlphaNoise; uniform float4 _AlphaNoise_ST;
            uniform float _AlphaAdder;
            uniform float _AlphaPower;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                float4 _AlphaNoise_var = tex2D(_AlphaNoise,TRANSFORM_TEX(i.uv0, _AlphaNoise));
                clip((_AlphaTexture_var.r*(_AlphaNoise_var.rgb+max(0.0,min((1.0 - pow((1.0-max(0,dot(normalDirection, viewDirection))),_AlphaPower)),1.0))+_AlphaAdder)) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
