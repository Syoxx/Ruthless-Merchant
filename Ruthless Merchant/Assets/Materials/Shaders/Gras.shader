// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34513,y:32708,varname:node_9361,prsc:2|emission-3113-OUT,custl-6946-OUT,clip-8200-R,voffset-4598-OUT,tess-2663-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:32962,y:33141,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:32962,y:32934,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:32467,y:32744,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_Dot,id:7782,x:32709,y:32797,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-328-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:31933,y:32381,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1941,x:32871,y:32744,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-544-OUT,B-7782-OUT;n:type:ShaderForge.SFN_Color,id:5927,x:31933,y:32204,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5927,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:32159,y:32320,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB;n:type:ShaderForge.SFN_Posterize,id:1686,x:33822,y:32909,varname:node_1686,prsc:2|IN-1596-OUT,STPS-7993-OUT;n:type:ShaderForge.SFN_Vector1,id:7993,x:33596,y:33102,varname:node_7993,prsc:2,v1:6;n:type:ShaderForge.SFN_Lerp,id:6946,x:34051,y:32835,varname:node_6946,prsc:2|A-3747-OUT,B-1686-OUT,T-639-OUT;n:type:ShaderForge.SFN_Slider,id:639,x:33368,y:32813,ptovrint:False,ptlb:Posterize-Effect,ptin:_PosterizeEffect,varname:node_639,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Min,id:1596,x:33596,y:32955,varname:node_1596,prsc:2|A-3747-OUT,B-7981-OUT;n:type:ShaderForge.SFN_Vector1,id:7981,x:33388,y:33167,varname:node_7981,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2945,x:32962,y:33076,ptovrint:False,ptlb:Posterize-Multiplier,ptin:_PosterizeMultiplier,varname:_noiseAdder_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3747,x:33200,y:32924,cmnt:Attenuate and Color,varname:node_3747,prsc:2|A-1941-OUT,B-2945-OUT,C-3406-RGB,D-8068-OUT;n:type:ShaderForge.SFN_AmbientLight,id:2701,x:32159,y:32184,varname:node_2701,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3113,x:32490,y:32419,cmnt:Diffuse Color,varname:node_3113,prsc:2|A-1739-OUT,B-2701-RGB,C-544-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1739,x:32159,y:32116,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:node_1739,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:8200,x:34198,y:32988,ptovrint:False,ptlb:AlphaTexture,ptin:_AlphaTexture,varname:node_8200,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector3,id:328,x:32467,y:32942,varname:node_328,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_FragmentPosition,id:8874,x:33508,y:33254,varname:node_8874,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:3687,x:33508,y:33378,varname:node_3687,prsc:2;n:type:ShaderForge.SFN_Normalize,id:309,x:33864,y:33339,varname:node_309,prsc:2|IN-2750-OUT;n:type:ShaderForge.SFN_Subtract,id:2750,x:33687,y:33339,varname:node_2750,prsc:2|A-8874-XYZ,B-3687-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:7749,x:34035,y:33339,varname:node_7749,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-309-OUT;n:type:ShaderForge.SFN_Add,id:664,x:34206,y:33377,varname:node_664,prsc:2|A-7749-G,B-266-OUT;n:type:ShaderForge.SFN_Vector1,id:266,x:34035,y:33486,varname:node_266,prsc:2,v1:1;n:type:ShaderForge.SFN_Divide,id:5330,x:34371,y:33377,varname:node_5330,prsc:2|A-664-OUT,B-2793-OUT;n:type:ShaderForge.SFN_Vector1,id:2793,x:34206,y:33502,varname:node_2793,prsc:2,v1:2;n:type:ShaderForge.SFN_Time,id:1427,x:33508,y:33680,varname:node_1427,prsc:2;n:type:ShaderForge.SFN_Sin,id:7565,x:34091,y:33635,varname:node_7565,prsc:2|IN-2946-OUT;n:type:ShaderForge.SFN_Append,id:4122,x:34281,y:33664,varname:node_4122,prsc:2|A-7565-OUT,B-9225-OUT,C-9225-OUT;n:type:ShaderForge.SFN_Vector1,id:9225,x:34091,y:33776,varname:node_9225,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:4598,x:34961,y:33313,varname:node_4598,prsc:2|A-4190-OUT,B-3763-OUT,T-4733-OUT;n:type:ShaderForge.SFN_Vector3,id:4190,x:34422,y:33225,varname:node_4190,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Multiply,id:3763,x:34680,y:33640,varname:node_3763,prsc:2|A-4122-OUT,B-932-OUT;n:type:ShaderForge.SFN_ValueProperty,id:932,x:34428,y:33723,ptovrint:False,ptlb:MoveValue,ptin:_MoveValue,varname:node_932,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:2946,x:33925,y:33803,varname:node_2946,prsc:2|A-4951-OUT,B-9457-OUT,C-5856-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8419,x:33475,y:33841,varname:node_8419,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9457,x:33760,y:34065,varname:node_9457,prsc:2|A-8419-Y,B-4133-OUT;n:type:ShaderForge.SFN_Pi,id:4133,x:33508,y:33989,varname:node_4133,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4951,x:33735,y:33658,varname:node_4951,prsc:2|A-6667-OUT,B-1427-T;n:type:ShaderForge.SFN_Vector1,id:6667,x:33508,y:33619,varname:node_6667,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:2663,x:34255,y:33225,varname:node_2663,prsc:2,v1:6;n:type:ShaderForge.SFN_ValueProperty,id:5427,x:34359,y:33549,ptovrint:False,ptlb:node_5427,ptin:_node_5427,varname:node_5427,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4733,x:34547,y:33377,varname:node_4733,prsc:2|A-5330-OUT,B-5427-OUT;n:type:ShaderForge.SFN_Multiply,id:5856,x:33760,y:33916,varname:node_5856,prsc:2|A-8419-X,B-8819-OUT;n:type:ShaderForge.SFN_Vector1,id:8819,x:33634,y:33806,varname:node_8819,prsc:2,v1:4;proporder:851-5927-639-2945-1739-8200-932-5427;pass:END;sub:END;*/

Shader "Shader Forge/Gras" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _PosterizeEffect ("Posterize-Effect", Range(0, 1)) = 1
        _PosterizeMultiplier ("Posterize-Multiplier", Float ) = 1
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 1
        _AlphaTexture ("AlphaTexture", 2D) = "white" {}
        _MoveValue ("MoveValue", Float ) = 0
        _node_5427 ("node_5427", Float ) = 0
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
            uniform float _node_5427;
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
                v.vertex.xyz += lerp(float3(0,0,0),(float3(sin((((-1.0)*node_1427.g)+(mul(unity_ObjectToWorld, v.vertex).g*3.141592654)+(mul(unity_ObjectToWorld, v.vertex).r*4.0))),node_9225,node_9225)*_MoveValue),(((normalize((mul(unity_ObjectToWorld, v.vertex).rgb-objPos.rgb)).rg.g+1.0)/2.0)*_node_5427));
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
                float node_7993 = 6.0;
                float3 finalColor = emissive + lerp(node_3747,floor(min(node_3747,1.0) * node_7993) / (node_7993 - 1),_PosterizeEffect);
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
            uniform float _node_5427;
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
                v.vertex.xyz += lerp(float3(0,0,0),(float3(sin((((-1.0)*node_1427.g)+(mul(unity_ObjectToWorld, v.vertex).g*3.141592654)+(mul(unity_ObjectToWorld, v.vertex).r*4.0))),node_9225,node_9225)*_MoveValue),(((normalize((mul(unity_ObjectToWorld, v.vertex).rgb-objPos.rgb)).rg.g+1.0)/2.0)*_node_5427));
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
                float node_7993 = 6.0;
                float3 finalColor = lerp(node_3747,floor(min(node_3747,1.0) * node_7993) / (node_7993 - 1),_PosterizeEffect);
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
            uniform float _node_5427;
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
                v.vertex.xyz += lerp(float3(0,0,0),(float3(sin((((-1.0)*node_1427.g)+(mul(unity_ObjectToWorld, v.vertex).g*3.141592654)+(mul(unity_ObjectToWorld, v.vertex).r*4.0))),node_9225,node_9225)*_MoveValue),(((normalize((mul(unity_ObjectToWorld, v.vertex).rgb-objPos.rgb)).rg.g+1.0)/2.0)*_node_5427));
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
