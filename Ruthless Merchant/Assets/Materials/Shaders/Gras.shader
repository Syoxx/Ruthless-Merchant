// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34513,y:32708,varname:node_9361,prsc:2|emission-3113-OUT,custl-6946-OUT,clip-8200-R,voffset-4598-OUT,tess-2663-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:33471,y:33038,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:33471,y:32916,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:33096,y:32657,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_Dot,id:7782,x:33292,y:32724,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-328-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:33096,y:32309,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1941,x:33471,y:32699,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-544-OUT,B-7782-OUT;n:type:ShaderForge.SFN_Color,id:5927,x:33096,y:32505,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5927,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:33292,y:32580,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB;n:type:ShaderForge.SFN_Posterize,id:1686,x:34082,y:32887,varname:node_1686,prsc:2|IN-1596-OUT,STPS-6799-OUT;n:type:ShaderForge.SFN_Lerp,id:6946,x:34315,y:32844,varname:node_6946,prsc:2|A-3747-OUT,B-1686-OUT,T-639-OUT;n:type:ShaderForge.SFN_Slider,id:639,x:33796,y:33135,ptovrint:False,ptlb:Posterize-Effect,ptin:_PosterizeEffect,varname:node_639,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Min,id:1596,x:33875,y:32887,varname:node_1596,prsc:2|A-3747-OUT,B-7981-OUT;n:type:ShaderForge.SFN_Vector1,id:7981,x:33676,y:32976,varname:node_7981,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2945,x:33471,y:32849,ptovrint:False,ptlb:Posterize-Multiplier,ptin:_PosterizeMultiplier,varname:_noiseAdder_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3747,x:33676,y:32838,cmnt:Attenuate and Color,varname:node_3747,prsc:2|A-1941-OUT,B-2945-OUT,C-3406-RGB,D-8068-OUT;n:type:ShaderForge.SFN_AmbientLight,id:2701,x:33292,y:32432,varname:node_2701,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3113,x:33471,y:32432,cmnt:Diffuse Color,varname:node_3113,prsc:2|A-1739-OUT,B-2701-RGB,C-544-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1739,x:33292,y:32360,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:node_1739,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:8200,x:34315,y:33015,ptovrint:False,ptlb:AlphaTexture,ptin:_AlphaTexture,varname:node_8200,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector3,id:328,x:33096,y:32790,varname:node_328,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_FragmentPosition,id:8874,x:32854,y:33645,varname:node_8874,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:3687,x:32854,y:33769,varname:node_3687,prsc:2;n:type:ShaderForge.SFN_Normalize,id:309,x:33184,y:33730,varname:node_309,prsc:2|IN-2750-OUT;n:type:ShaderForge.SFN_Subtract,id:2750,x:33033,y:33730,varname:node_2750,prsc:2|A-8874-XYZ,B-3687-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:7749,x:33353,y:33730,varname:node_7749,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-309-OUT;n:type:ShaderForge.SFN_Add,id:664,x:33552,y:33730,varname:node_664,prsc:2|A-7749-G,B-266-OUT;n:type:ShaderForge.SFN_Vector1,id:266,x:33353,y:33904,varname:node_266,prsc:2,v1:1;n:type:ShaderForge.SFN_Divide,id:5330,x:33732,y:33730,varname:node_5330,prsc:2|A-664-OUT,B-2793-OUT;n:type:ShaderForge.SFN_Vector1,id:2793,x:33552,y:33854,varname:node_2793,prsc:2,v1:2;n:type:ShaderForge.SFN_Time,id:1427,x:33207,y:33241,varname:node_1427,prsc:2;n:type:ShaderForge.SFN_Sin,id:7565,x:33764,y:33322,varname:node_7565,prsc:2|IN-2946-OUT;n:type:ShaderForge.SFN_Append,id:4122,x:33939,y:33322,varname:node_4122,prsc:2|A-7565-OUT,B-9225-OUT,C-9225-OUT;n:type:ShaderForge.SFN_Vector1,id:9225,x:33764,y:33448,varname:node_9225,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:4598,x:34315,y:33177,varname:node_4598,prsc:2|A-4190-OUT,B-3763-OUT,T-195-OUT;n:type:ShaderForge.SFN_Vector3,id:4190,x:34103,y:33215,varname:node_4190,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Multiply,id:3763,x:34103,y:33322,varname:node_3763,prsc:2|A-4122-OUT,B-932-OUT;n:type:ShaderForge.SFN_ValueProperty,id:932,x:33939,y:33479,ptovrint:False,ptlb:MoveValue,ptin:_MoveValue,varname:node_932,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:2946,x:33589,y:33322,varname:node_2946,prsc:2|A-4951-OUT,B-9457-OUT,C-1713-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8419,x:33207,y:33515,varname:node_8419,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9457,x:33396,y:33377,varname:node_9457,prsc:2|A-4133-OUT,B-8419-Y;n:type:ShaderForge.SFN_Pi,id:4133,x:33240,y:33377,varname:node_4133,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4951,x:33396,y:33241,varname:node_4951,prsc:2|A-6667-OUT,B-1427-T;n:type:ShaderForge.SFN_Vector1,id:6667,x:33207,y:33173,varname:node_6667,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:2663,x:34315,y:33322,varname:node_2663,prsc:2,v1:6;n:type:ShaderForge.SFN_Slider,id:6813,x:33517,y:33978,ptovrint:False,ptlb:MovePosition,ptin:_MovePosition,varname:node_6813,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6097428,max:1;n:type:ShaderForge.SFN_Subtract,id:9966,x:33905,y:33767,varname:node_9966,prsc:2|A-5330-OUT,B-6813-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:195,x:34108,y:33798,varname:node_195,prsc:2,min:0,max:1|IN-9966-OUT;n:type:ShaderForge.SFN_Multiply,id:1713,x:33396,y:33515,varname:node_1713,prsc:2|A-8419-X,B-8419-Z,C-4059-OUT;n:type:ShaderForge.SFN_Vector1,id:4059,x:33207,y:33651,varname:node_4059,prsc:2,v1:8;n:type:ShaderForge.SFN_ValueProperty,id:6799,x:33875,y:33035,ptovrint:False,ptlb:Posterize-Number,ptin:_PosterizeNumber,varname:node_6799,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;proporder:851-5927-1739-639-2945-932-8200-6813-6799;pass:END;sub:END;*/

Shader "Shader Forge/Gras" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 1
        _PosterizeEffect ("Posterize-Effect", Range(0, 1)) = 1
        _PosterizeMultiplier ("Posterize-Multiplier", Float ) = 1
        _MoveValue ("MoveValue", Float ) = 1
        _AlphaTexture ("AlphaTexture", 2D) = "white" {}
        _MovePosition ("MovePosition", Range(0, 1)) = 0.6097428
        _PosterizeNumber ("Posterize-Number", Float ) = 4
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
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform float _PosterizeEffect;
            uniform float _PosterizeMultiplier;
            uniform float _AmbientMultiplier;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _MoveValue;
            uniform float _MovePosition;
            uniform float _PosterizeNumber;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1427 = _Time;
                float node_9225 = 0.0;
                v.vertex.xyz += lerp(float3(0,0,0),(float3(sin((((-1.0)*node_1427.g)+(3.141592654*mul(unity_ObjectToWorld, v.vertex).g)+(mul(unity_ObjectToWorld, v.vertex).r*mul(unity_ObjectToWorld, v.vertex).b*8.0))),node_9225,node_9225)*_MoveValue),clamp((((normalize((mul(unity_ObjectToWorld, v.vertex).rgb-objPos.rgb)).rg.g+1.0)/2.0)-_MovePosition),0,1));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return 6.0;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                clip(_AlphaTexture_var.r - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 emissive = (_AmbientMultiplier*UNITY_LIGHTMODEL_AMBIENT.rgb*node_544);
                float3 node_3747 = ((node_544*max(0,dot(lightDirection,float3(0,1,0))))*_PosterizeMultiplier*_LightColor0.rgb*attenuation); // Attenuate and Color
                float3 finalColor = emissive + lerp(node_3747,floor(min(node_3747,1.0) * _PosterizeNumber) / (_PosterizeNumber - 1),_PosterizeEffect);
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
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform float _PosterizeEffect;
            uniform float _PosterizeMultiplier;
            uniform float _AmbientMultiplier;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _MoveValue;
            uniform float _MovePosition;
            uniform float _PosterizeNumber;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1427 = _Time;
                float node_9225 = 0.0;
                v.vertex.xyz += lerp(float3(0,0,0),(float3(sin((((-1.0)*node_1427.g)+(3.141592654*mul(unity_ObjectToWorld, v.vertex).g)+(mul(unity_ObjectToWorld, v.vertex).r*mul(unity_ObjectToWorld, v.vertex).b*8.0))),node_9225,node_9225)*_MoveValue),clamp((((normalize((mul(unity_ObjectToWorld, v.vertex).rgb-objPos.rgb)).rg.g+1.0)/2.0)-_MovePosition),0,1));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return 6.0;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                clip(_AlphaTexture_var.r - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 node_3747 = ((node_544*max(0,dot(lightDirection,float3(0,1,0))))*_PosterizeMultiplier*_LightColor0.rgb*attenuation); // Attenuate and Color
                float3 finalColor = lerp(node_3747,floor(min(node_3747,1.0) * _PosterizeNumber) / (_PosterizeNumber - 1),_PosterizeEffect);
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
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _MoveValue;
            uniform float _MovePosition;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
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
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1427 = _Time;
                float node_9225 = 0.0;
                v.vertex.xyz += lerp(float3(0,0,0),(float3(sin((((-1.0)*node_1427.g)+(3.141592654*mul(unity_ObjectToWorld, v.vertex).g)+(mul(unity_ObjectToWorld, v.vertex).r*mul(unity_ObjectToWorld, v.vertex).b*8.0))),node_9225,node_9225)*_MoveValue),clamp((((normalize((mul(unity_ObjectToWorld, v.vertex).rgb-objPos.rgb)).rg.g+1.0)/2.0)-_MovePosition),0,1));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return 6.0;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                clip(_AlphaTexture_var.r - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
