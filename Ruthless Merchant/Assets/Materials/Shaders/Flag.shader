// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33785,y:32460,varname:node_9361,prsc:2|emission-9476-OUT,custl-544-OUT,olwid-736-OUT,voffset-7146-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:32641,y:33026,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:32641,y:32892,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:31562,y:32816,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:9684,x:31562,y:32951,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:7782,x:31785,y:32891,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-9684-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:32815,y:32435,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:5927,x:32813,y:32641,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5927,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:33256,y:32754,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB,C-3839-OUT,D-1347-OUT;n:type:ShaderForge.SFN_Lerp,id:6191,x:32552,y:32745,varname:node_6191,prsc:2|A-5003-OUT,B-1907-OUT,T-9148-OUT;n:type:ShaderForge.SFN_Slider,id:9148,x:32177,y:33199,ptovrint:False,ptlb:Posterizer,ptin:_Posterizer,varname:node_9148,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:2301,x:32827,y:32873,varname:node_2301,prsc:2|A-6191-OUT,B-3406-RGB,C-8068-OUT;n:type:ShaderForge.SFN_Clamp01,id:3839,x:33028,y:32823,varname:node_3839,prsc:2|IN-2301-OUT;n:type:ShaderForge.SFN_Add,id:4040,x:31984,y:32894,varname:node_4040,prsc:2|A-7782-OUT,B-8378-OUT;n:type:ShaderForge.SFN_Clamp01,id:5003,x:32163,y:32814,varname:node_5003,prsc:2|IN-4040-OUT;n:type:ShaderForge.SFN_AmbientLight,id:2721,x:33006,y:31950,varname:node_2721,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1608,x:33277,y:32148,varname:node_1608,prsc:2|A-2721-RGB,B-5922-OUT,C-5927-RGB,D-7662-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5922,x:33006,y:32109,ptovrint:False,ptlb:EmissionMultiplier,ptin:_EmissionMultiplier,varname:node_5922,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.4;n:type:ShaderForge.SFN_Power,id:7662,x:33051,y:32336,varname:node_7662,prsc:2|VAL-851-RGB,EXP-4311-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4311,x:32815,y:32336,ptovrint:False,ptlb:TexturePower,ptin:_TexturePower,varname:node_4311,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:8317,x:33251,y:33006,ptovrint:False,ptlb:Outline,ptin:_Outline,varname:node_8317,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Fresnel,id:8796,x:32249,y:32243,varname:node_8796,prsc:2;n:type:ShaderForge.SFN_Color,id:3915,x:32249,y:32089,ptovrint:False,ptlb:SelectColor,ptin:_SelectColor,varname:node_3915,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:6166,x:32442,y:32168,varname:node_6166,prsc:2|A-3915-RGB,B-8796-OUT,C-2791-RGB,D-2040-OUT;n:type:ShaderForge.SFN_Add,id:9476,x:33473,y:32360,varname:node_9476,prsc:2|A-1608-OUT,B-8234-OUT;n:type:ShaderForge.SFN_Tex2d,id:2791,x:32249,y:32393,ptovrint:False,ptlb:SelectEffectNoiseTexture,ptin:_SelectEffectNoiseTexture,varname:node_2791,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2d657126723b13d4c8e7ffd3076133b0,ntxv:0,isnm:False|UVIN-7708-OUT;n:type:ShaderForge.SFN_Append,id:7708,x:32075,y:32393,varname:node_7708,prsc:2|A-4808-U,B-1865-OUT;n:type:ShaderForge.SFN_Add,id:1865,x:31931,y:32464,varname:node_1865,prsc:2|A-4808-V,B-3943-TSL;n:type:ShaderForge.SFN_Time,id:3943,x:31761,y:32505,varname:node_3943,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4608,x:32796,y:32179,varname:node_4608,prsc:2|A-4010-OUT,B-7709-OUT;n:type:ShaderForge.SFN_Slider,id:7709,x:32417,y:32674,ptovrint:False,ptlb:SelectEffect,ptin:_SelectEffect,varname:node_7709,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_TexCoord,id:4808,x:31761,y:32359,varname:node_4808,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:4010,x:32613,y:32179,varname:node_4010,prsc:2|A-6166-OUT,B-247-OUT;n:type:ShaderForge.SFN_Clamp01,id:8234,x:33006,y:32179,varname:node_8234,prsc:2|IN-4608-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2040,x:32249,y:32587,ptovrint:False,ptlb:SelectEffectStrength,ptin:_SelectEffectStrength,varname:_SelectSize_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Fresnel,id:4474,x:32289,y:33393,varname:node_4474,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:975,x:32468,y:33393,varname:node_975,prsc:2|IN-4474-OUT;n:type:ShaderForge.SFN_Lerp,id:8990,x:32827,y:33248,varname:node_8990,prsc:2|A-3230-OUT,B-5633-OUT,T-7709-OUT;n:type:ShaderForge.SFN_Vector3,id:3230,x:32645,y:33232,varname:node_3230,prsc:2,v1:1,v2:1,v3:1;n:type:ShaderForge.SFN_Clamp01,id:1347,x:33028,y:33193,varname:node_1347,prsc:2|IN-8990-OUT;n:type:ShaderForge.SFN_Power,id:3742,x:32645,y:33393,varname:node_3742,prsc:2|VAL-975-OUT,EXP-4070-OUT;n:type:ShaderForge.SFN_Vector1,id:4070,x:32468,y:33544,varname:node_4070,prsc:2,v1:5;n:type:ShaderForge.SFN_Multiply,id:5633,x:32827,y:33425,varname:node_5633,prsc:2|A-3742-OUT,B-4070-OUT;n:type:ShaderForge.SFN_Divide,id:736,x:33448,y:33006,varname:node_736,prsc:2|A-8317-OUT,B-4037-OUT;n:type:ShaderForge.SFN_Vector1,id:4037,x:33251,y:33090,varname:node_4037,prsc:2,v1:100;n:type:ShaderForge.SFN_OneMinus,id:8378,x:31779,y:33203,varname:node_8378,prsc:2|IN-3885-OUT;n:type:ShaderForge.SFN_Slider,id:3885,x:31412,y:33280,ptovrint:False,ptlb:ShadowSize,ptin:_ShadowSize,varname:node_3885,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Slider,id:4752,x:31604,y:32649,ptovrint:False,ptlb:SelectEffectSize,ptin:_SelectEffectSize,varname:_SelectSlider_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8007979,max:1;n:type:ShaderForge.SFN_Subtract,id:247,x:32246,y:32656,varname:node_247,prsc:2|A-7324-OUT,B-6743-OUT;n:type:ShaderForge.SFN_Vector1,id:7324,x:32045,y:32628,varname:node_7324,prsc:2,v1:0;n:type:ShaderForge.SFN_OneMinus,id:6743,x:32045,y:32686,varname:node_6743,prsc:2|IN-4752-OUT;n:type:ShaderForge.SFN_Step,id:1907,x:32346,y:32874,varname:node_1907,prsc:2|A-7225-OUT,B-5003-OUT;n:type:ShaderForge.SFN_Slider,id:7225,x:32034,y:33044,ptovrint:False,ptlb:PosterizeRange,ptin:_PosterizeRange,varname:_Posterizer_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Sin,id:9209,x:33539,y:33344,varname:node_9209,prsc:2|IN-3982-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8138,x:33028,y:33472,varname:node_8138,prsc:2;n:type:ShaderForge.SFN_Time,id:4059,x:33200,y:33193,varname:node_4059,prsc:2;n:type:ShaderForge.SFN_Add,id:3982,x:33375,y:33365,varname:node_3982,prsc:2|A-4692-OUT,B-6849-OUT;n:type:ShaderForge.SFN_Multiply,id:7146,x:33765,y:33116,varname:node_7146,prsc:2|A-1890-XYZ,B-9209-OUT,C-2339-OUT,D-3451-R;n:type:ShaderForge.SFN_Multiply,id:6849,x:33200,y:33490,varname:node_6849,prsc:2|A-8138-X,B-4719-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4719,x:33028,y:33642,ptovrint:False,ptlb:WaveSize,ptin:_WaveSize,varname:node_4719,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2339,x:33539,y:33553,ptovrint:False,ptlb:WaveStrength,ptin:_WaveStrength,varname:node_2339,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3451,x:33539,y:33652,ptovrint:False,ptlb:WaveMap,ptin:_WaveMap,varname:node_3451,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:1312,x:33028,y:33398,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:node_1312,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:4692,x:33200,y:33339,varname:node_4692,prsc:2|A-4059-T,B-1312-OUT;n:type:ShaderForge.SFN_Vector4Property,id:1890,x:33509,y:33180,ptovrint:False,ptlb:WaveDirection,ptin:_WaveDirection,varname:node_1890,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:0,v3:0,v4:0;proporder:851-5927-9148-7225-3885-5922-4311-8317-3915-2791-7709-2040-4752-4719-2339-3451-1312-1890;pass:END;sub:END;*/

Shader "Shader Forge/Flag" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Posterizer ("Posterizer", Range(0, 1)) = 0
        _PosterizeRange ("PosterizeRange", Range(0, 1)) = 0.5
        _ShadowSize ("ShadowSize", Range(0, 2)) = 1
        _EmissionMultiplier ("EmissionMultiplier", Float ) = 0.4
        _TexturePower ("TexturePower", Float ) = 2
        _Outline ("Outline", Float ) = 0
        _SelectColor ("SelectColor", Color) = (0,1,0,1)
        _SelectEffectNoiseTexture ("SelectEffectNoiseTexture", 2D) = "white" {}
        _SelectEffect ("SelectEffect", Range(0, 1)) = 0
        _SelectEffectStrength ("SelectEffectStrength", Float ) = 2
        _SelectEffectSize ("SelectEffectSize", Range(0, 1)) = 0.8007979
        _WaveSize ("WaveSize", Float ) = 1
        _WaveStrength ("WaveStrength", Float ) = 1
        _WaveMap ("WaveMap", 2D) = "white" {}
        _WaveSpeed ("WaveSpeed", Float ) = 1
        _WaveDirection ("WaveDirection", Vector) = (1,0,0,0)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Outline;
            uniform float _WaveSize;
            uniform float _WaveStrength;
            uniform sampler2D _WaveMap; uniform float4 _WaveMap_ST;
            uniform float _WaveSpeed;
            uniform float4 _WaveDirection;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_4059 = _Time;
                float4 _WaveMap_var = tex2Dlod(_WaveMap,float4(TRANSFORM_TEX(o.uv0, _WaveMap),0.0,0));
                v.vertex.xyz += (_WaveDirection.rgb*sin(((node_4059.g*_WaveSpeed)+(mul(unity_ObjectToWorld, v.vertex).r*_WaveSize)))*_WaveStrength*_WaveMap_var.r);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*(_Outline/100.0),1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                return fixed4(float3(0,0,0),0);
            }
            ENDCG
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
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Posterizer;
            uniform float _EmissionMultiplier;
            uniform float _TexturePower;
            uniform float4 _SelectColor;
            uniform sampler2D _SelectEffectNoiseTexture; uniform float4 _SelectEffectNoiseTexture_ST;
            uniform float _SelectEffect;
            uniform float _SelectEffectStrength;
            uniform float _ShadowSize;
            uniform float _SelectEffectSize;
            uniform float _PosterizeRange;
            uniform float _WaveSize;
            uniform float _WaveStrength;
            uniform sampler2D _WaveMap; uniform float4 _WaveMap_ST;
            uniform float _WaveSpeed;
            uniform float4 _WaveDirection;
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
                float4 node_4059 = _Time;
                float4 _WaveMap_var = tex2Dlod(_WaveMap,float4(TRANSFORM_TEX(o.uv0, _WaveMap),0.0,0));
                v.vertex.xyz += (_WaveDirection.rgb*sin(((node_4059.g*_WaveSpeed)+(mul(unity_ObjectToWorld, v.vertex).r*_WaveSize)))*_WaveStrength*_WaveMap_var.r);
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
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
////// Emissive:
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float4 node_3943 = _Time;
                float2 node_7708 = float2(i.uv0.r,(i.uv0.g+node_3943.r));
                float4 _SelectEffectNoiseTexture_var = tex2D(_SelectEffectNoiseTexture,TRANSFORM_TEX(node_7708, _SelectEffectNoiseTexture));
                float3 emissive = ((UNITY_LIGHTMODEL_AMBIENT.rgb*_EmissionMultiplier*_Color.rgb*pow(_Texture_var.rgb,_TexturePower))+saturate((((_SelectColor.rgb*(1.0-max(0,dot(normalDirection, viewDirection)))*_SelectEffectNoiseTexture_var.rgb*_SelectEffectStrength)+(0.0-(1.0 - _SelectEffectSize)))*_SelectEffect)));
                float node_5003 = saturate((max(0,dot(lightDirection,normalDirection))+(1.0 - _ShadowSize)));
                float node_4070 = 5.0;
                float node_5633 = (pow((1.0 - (1.0-max(0,dot(normalDirection, viewDirection)))),node_4070)*node_4070);
                float3 finalColor = emissive + (_Texture_var.rgb*_Color.rgb*saturate((lerp(node_5003,step(_PosterizeRange,node_5003),_Posterizer)*_LightColor0.rgb*attenuation))*saturate(lerp(float3(1,1,1),float3(node_5633,node_5633,node_5633),_SelectEffect)));
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
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _Color;
            uniform float _Posterizer;
            uniform float _EmissionMultiplier;
            uniform float _TexturePower;
            uniform float4 _SelectColor;
            uniform sampler2D _SelectEffectNoiseTexture; uniform float4 _SelectEffectNoiseTexture_ST;
            uniform float _SelectEffect;
            uniform float _SelectEffectStrength;
            uniform float _ShadowSize;
            uniform float _SelectEffectSize;
            uniform float _PosterizeRange;
            uniform float _WaveSize;
            uniform float _WaveStrength;
            uniform sampler2D _WaveMap; uniform float4 _WaveMap_ST;
            uniform float _WaveSpeed;
            uniform float4 _WaveDirection;
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
                float4 node_4059 = _Time;
                float4 _WaveMap_var = tex2Dlod(_WaveMap,float4(TRANSFORM_TEX(o.uv0, _WaveMap),0.0,0));
                v.vertex.xyz += (_WaveDirection.rgb*sin(((node_4059.g*_WaveSpeed)+(mul(unity_ObjectToWorld, v.vertex).r*_WaveSize)))*_WaveStrength*_WaveMap_var.r);
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
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float node_5003 = saturate((max(0,dot(lightDirection,normalDirection))+(1.0 - _ShadowSize)));
                float node_4070 = 5.0;
                float node_5633 = (pow((1.0 - (1.0-max(0,dot(normalDirection, viewDirection)))),node_4070)*node_4070);
                float3 finalColor = (_Texture_var.rgb*_Color.rgb*saturate((lerp(node_5003,step(_PosterizeRange,node_5003),_Posterizer)*_LightColor0.rgb*attenuation))*saturate(lerp(float3(1,1,1),float3(node_5633,node_5633,node_5633),_SelectEffect)));
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
            uniform float _WaveSize;
            uniform float _WaveStrength;
            uniform sampler2D _WaveMap; uniform float4 _WaveMap_ST;
            uniform float _WaveSpeed;
            uniform float4 _WaveDirection;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_4059 = _Time;
                float4 _WaveMap_var = tex2Dlod(_WaveMap,float4(TRANSFORM_TEX(o.uv0, _WaveMap),0.0,0));
                v.vertex.xyz += (_WaveDirection.rgb*sin(((node_4059.g*_WaveSpeed)+(mul(unity_ObjectToWorld, v.vertex).r*_WaveSize)))*_WaveStrength*_WaveMap_var.r);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
