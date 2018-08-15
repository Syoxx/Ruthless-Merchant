// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:1,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:36228,y:32433,varname:node_9361,prsc:2|custl-1093-RGB,olcol-1093-RGB,voffset-254-OUT,tess-8805-OUT;n:type:ShaderForge.SFN_Time,id:8293,x:34434,y:32597,varname:node_8293,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1625,x:35585,y:32403,varname:node_1625,prsc:2|A-8621-OUT,B-8170-OUT;n:type:ShaderForge.SFN_Slider,id:8621,x:35199,y:32372,ptovrint:False,ptlb:WaveStrength,ptin:_WaveStrength,varname:_WaveStrength,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ValueProperty,id:5642,x:35786,y:33123,ptovrint:False,ptlb:Tessellation,ptin:_Tessellation,varname:_Tessellation,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Max,id:8805,x:35973,y:33030,varname:node_8805,prsc:2|A-2418-OUT,B-5642-OUT;n:type:ShaderForge.SFN_Vector1,id:2418,x:35786,y:33030,varname:node_2418,prsc:2,v1:1;n:type:ShaderForge.SFN_NormalVector,id:8905,x:35585,y:32546,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:7409,x:35784,y:32459,varname:node_7409,prsc:2|A-1625-OUT,B-8905-OUT;n:type:ShaderForge.SFN_Sin,id:3152,x:34996,y:32515,varname:node_3152,prsc:2|IN-3936-OUT;n:type:ShaderForge.SFN_Add,id:3936,x:34816,y:32515,varname:node_3936,prsc:2|A-7754-OUT,B-6113-OUT;n:type:ShaderForge.SFN_Multiply,id:7754,x:34625,y:32452,varname:node_7754,prsc:2|A-3363-R,B-7665-OUT,C-8777-OUT;n:type:ShaderForge.SFN_Pi,id:7665,x:34467,y:32412,varname:node_7665,prsc:2;n:type:ShaderForge.SFN_Vector1,id:8777,x:34434,y:32534,varname:node_8777,prsc:2,v1:4;n:type:ShaderForge.SFN_Multiply,id:6113,x:34625,y:32597,varname:node_6113,prsc:2|A-8293-T,B-8266-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8266,x:34434,y:32760,ptovrint:False,ptlb:Wave_speed_1,ptin:_Wave_speed_1,varname:node_8266,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3363,x:34434,y:32246,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_3363,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2d657126723b13d4c8e7ffd3076133b0,ntxv:0,isnm:False|UVIN-8739-OUT;n:type:ShaderForge.SFN_Add,id:9301,x:35177,y:32515,varname:node_9301,prsc:2|A-3152-OUT,B-4138-OUT;n:type:ShaderForge.SFN_Vector1,id:7742,x:35177,y:32645,varname:node_7742,prsc:2,v1:2;n:type:ShaderForge.SFN_Divide,id:8170,x:35356,y:32515,varname:node_8170,prsc:2|A-9301-OUT,B-7742-OUT;n:type:ShaderForge.SFN_Vector1,id:4138,x:34996,y:32645,varname:node_4138,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:254,x:35971,y:32603,varname:node_254,prsc:2|A-7409-OUT,B-6886-XYZ;n:type:ShaderForge.SFN_Vector4Property,id:6886,x:35784,y:32619,ptovrint:False,ptlb:Displacement,ptin:_Displacement,varname:node_6886,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Color,id:1093,x:35971,y:32449,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1093,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:9230,x:33921,y:32233,varname:node_9230,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector4Property,id:2611,x:33586,y:32158,ptovrint:False,ptlb:Flowspeed,ptin:_Flowspeed,varname:node_2611,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Time,id:6058,x:33586,y:32323,varname:node_6058,prsc:2;n:type:ShaderForge.SFN_Multiply,id:647,x:33887,y:32388,varname:node_647,prsc:2|A-2611-Y,B-6058-TSL;n:type:ShaderForge.SFN_Multiply,id:1027,x:33789,y:32168,varname:node_1027,prsc:2|A-2611-X,B-6058-TSL;n:type:ShaderForge.SFN_Add,id:4073,x:34091,y:32320,varname:node_4073,prsc:2|A-647-OUT,B-9230-V;n:type:ShaderForge.SFN_Add,id:8221,x:34091,y:32196,varname:node_8221,prsc:2|A-1027-OUT,B-9230-U;n:type:ShaderForge.SFN_Append,id:8739,x:34270,y:32277,varname:node_8739,prsc:2|A-8221-OUT,B-4073-OUT;proporder:8621-5642-8266-3363-6886-1093-2611;pass:END;sub:END;*/

Shader "Shader Forge/Water Shine" {
    Properties {
        _WaveStrength ("WaveStrength", Range(0, 1)) = 1
        _Tessellation ("Tessellation", Float ) = 1
        _Wave_speed_1 ("Wave_speed_1", Float ) = 1
        _Noise ("Noise", 2D) = "white" {}
        _Displacement ("Displacement", Vector) = (0,0,0,0)
        _Color ("Color", Color) = (1,1,1,1)
        _Flowspeed ("Flowspeed", Vector) = (0,0,0,0)
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
            Cull Front
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Wave_speed_1;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float4 _Displacement;
            uniform float4 _Color;
            uniform float4 _Flowspeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(-v.normal);
                float4 node_6058 = _Time;
                float2 node_8739 = float2(((_Flowspeed.r*node_6058.r)+o.uv0.r),((_Flowspeed.g*node_6058.r)+o.uv0.g));
                float4 _Noise_var = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8739, _Noise),0.0,0));
                float4 node_8293 = _Time;
                v.vertex.xyz += (((_WaveStrength*((sin(((_Noise_var.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))+1.0)/2.0))*v.normal)+_Displacement.rgb);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
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
                    return max(1.0,_Tessellation);
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
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
                float3 finalColor = _Color.rgb;
                fixed4 finalRGBA = fixed4(finalColor,1);
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
            Cull Front
            
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
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Wave_speed_1;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float4 _Displacement;
            uniform float4 _Flowspeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(-v.normal);
                float4 node_6058 = _Time;
                float2 node_8739 = float2(((_Flowspeed.r*node_6058.r)+o.uv0.r),((_Flowspeed.g*node_6058.r)+o.uv0.g));
                float4 _Noise_var = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8739, _Noise),0.0,0));
                float4 node_8293 = _Time;
                v.vertex.xyz += (((_WaveStrength*((sin(((_Noise_var.r*3.141592654*4.0)+(node_8293.g*_Wave_speed_1)))+1.0)/2.0))*v.normal)+_Displacement.rgb);
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
                    return max(1.0,_Tessellation);
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
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
