// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34903,y:32680,varname:node_9361,prsc:2|emission-9045-OUT,custl-6946-OUT,disp-3297-OUT,tess-8805-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:33823,y:32956,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3406,x:33823,y:32832,varname:node_3406,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6869,x:33465,y:32704,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_Dot,id:7782,x:33628,y:32774,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-1785-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:33449,y:32353,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1941,x:33823,y:32615,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-544-OUT,B-7782-OUT;n:type:ShaderForge.SFN_Color,id:5927,x:33449,y:32546,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:33646,y:32556,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB;n:type:ShaderForge.SFN_Posterize,id:1686,x:34525,y:32730,varname:node_1686,prsc:2|IN-1596-OUT,STPS-8613-OUT;n:type:ShaderForge.SFN_Lerp,id:6946,x:34702,y:32844,varname:node_6946,prsc:2|A-1686-OUT,B-3747-OUT,T-639-OUT;n:type:ShaderForge.SFN_Slider,id:639,x:34357,y:32939,ptovrint:False,ptlb:Posterize-Effect,ptin:_PosterizeEffect,varname:_PosterizeEffect,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Min,id:1596,x:34225,y:32721,varname:node_1596,prsc:2|A-7981-OUT,B-3747-OUT;n:type:ShaderForge.SFN_Vector1,id:7981,x:33990,y:32649,varname:node_7981,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2945,x:33823,y:32774,ptovrint:False,ptlb:Posterize-Multiplier,ptin:_PosterizeMultiplier,varname:_PosterizeMultiplier,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3747,x:34032,y:32867,cmnt:Attenuate and Color,varname:node_3747,prsc:2|A-1941-OUT,B-2945-OUT,C-3406-RGB,D-8068-OUT,E-3354-OUT;n:type:ShaderForge.SFN_AmbientLight,id:2701,x:33815,y:32288,varname:node_2701,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3113,x:34100,y:32491,cmnt:Diffuse Color,varname:node_3113,prsc:2|A-1739-OUT,B-2701-RGB,C-544-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1739,x:33815,y:32226,ptovrint:False,ptlb:Ambient Multiplier,ptin:_AmbientMultiplier,varname:_AmbientMultiplier,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_NormalVector,id:1785,x:33465,y:32834,prsc:2,pt:False;n:type:ShaderForge.SFN_Append,id:3297,x:34546,y:33046,varname:node_3297,prsc:2|A-9890-OUT,B-1625-OUT,C-9890-OUT;n:type:ShaderForge.SFN_Vector1,id:9890,x:34343,y:33128,varname:node_9890,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:4642,x:32857,y:33169,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:_Noise,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2d657126723b13d4c8e7ffd3076133b0,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Time,id:8293,x:33070,y:33327,varname:node_8293,prsc:2;n:type:ShaderForge.SFN_Sin,id:3009,x:33430,y:33261,varname:node_3009,prsc:2|IN-1236-OUT;n:type:ShaderForge.SFN_Add,id:1236,x:33268,y:33261,varname:node_1236,prsc:2|A-1121-OUT,B-8293-T;n:type:ShaderForge.SFN_Multiply,id:1121,x:33072,y:33186,varname:node_1121,prsc:2|A-4642-R,B-1107-OUT,C-8129-OUT;n:type:ShaderForge.SFN_Pi,id:1107,x:32874,y:33336,varname:node_1107,prsc:2;n:type:ShaderForge.SFN_Vector1,id:8129,x:32841,y:33448,varname:node_8129,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:1625,x:33611,y:33248,varname:node_1625,prsc:2|A-8621-OUT,B-3009-OUT;n:type:ShaderForge.SFN_Slider,id:8621,x:33230,y:33143,ptovrint:False,ptlb:WaveStrength,ptin:_WaveStrength,varname:_WaveStrength,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_OneMinus,id:6419,x:33611,y:33093,varname:node_6419,prsc:2|IN-8621-OUT;n:type:ShaderForge.SFN_Add,id:3354,x:33823,y:33103,varname:node_3354,prsc:2|A-6419-OUT,B-1625-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5642,x:34546,y:33280,ptovrint:False,ptlb:Tessellation,ptin:_Tessellation,varname:_Tessellation,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Max,id:8805,x:34720,y:33142,varname:node_8805,prsc:2|A-2418-OUT,B-5642-OUT;n:type:ShaderForge.SFN_Vector1,id:2418,x:34546,y:33186,varname:node_2418,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8613,x:34343,y:32816,ptovrint:False,ptlb:Posterize_Number,ptin:_Posterize_Number,varname:_Posterize_Number,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:6;n:type:ShaderForge.SFN_Add,id:9045,x:34525,y:32580,varname:node_9045,prsc:2|A-5935-OUT,B-3113-OUT;n:type:ShaderForge.SFN_Fresnel,id:6620,x:34100,y:32246,varname:node_6620,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3206,x:34100,y:32408,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:_Fresnel,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:5935,x:34294,y:32396,varname:node_5935,prsc:2|VAL-6620-OUT,EXP-3206-OUT;proporder:851-5927-1739-639-8613-2945-4642-8621-5642-3206;pass:END;sub:END;*/

Shader "Shader Forge/Water" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _AmbientMultiplier ("Ambient Multiplier", Float ) = 1
        _PosterizeEffect ("Posterize-Effect", Range(0, 1)) = 1
        _Posterize_Number ("Posterize_Number", Float ) = 6
        _PosterizeMultiplier ("Posterize-Multiplier", Float ) = 1
        _Noise ("Noise", 2D) = "bump" {}
        _WaveStrength ("WaveStrength", Range(0, 1)) = 0.2
        _Tessellation ("Tessellation", Float ) = 1
        _Fresnel ("Fresnel", Float ) = 1
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
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Posterize_Number;
            uniform float _Fresnel;
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
                void displacement (inout VertexInput v){
                    float node_9890 = 0.0;
                    float3 _Noise_var = UnpackNormal(tex2Dlod(_Noise,float4(TRANSFORM_TEX(v.texcoord0, _Noise),0.0,0)));
                    float4 node_8293 = _Time;
                    float node_1625 = (_WaveStrength*sin(((_Noise_var.r*3.141592654*2.0)+node_8293.g)));
                    v.vertex.xyz += float3(node_9890,node_1625,node_9890);
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
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
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
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 emissive = (pow((1.0-max(0,dot(normalDirection, viewDirection))),_Fresnel)+(_AmbientMultiplier*UNITY_LIGHTMODEL_AMBIENT.rgb*node_544));
                float3 _Noise_var = UnpackNormal(tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise)));
                float4 node_8293 = _Time;
                float node_1625 = (_WaveStrength*sin(((_Noise_var.r*3.141592654*2.0)+node_8293.g)));
                float3 node_3747 = ((node_544*max(0,dot(lightDirection,i.normalDir)))*_PosterizeMultiplier*_LightColor0.rgb*attenuation*((1.0 - _WaveStrength)+node_1625)); // Attenuate and Color
                float3 finalColor = emissive + lerp(floor(min(1.0,node_3747) * _Posterize_Number) / (_Posterize_Number - 1),node_3747,_PosterizeEffect);
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
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _WaveStrength;
            uniform float _Tessellation;
            uniform float _Posterize_Number;
            uniform float _Fresnel;
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
                void displacement (inout VertexInput v){
                    float node_9890 = 0.0;
                    float3 _Noise_var = UnpackNormal(tex2Dlod(_Noise,float4(TRANSFORM_TEX(v.texcoord0, _Noise),0.0,0)));
                    float4 node_8293 = _Time;
                    float node_1625 = (_WaveStrength*sin(((_Noise_var.r*3.141592654*2.0)+node_8293.g)));
                    v.vertex.xyz += float3(node_9890,node_1625,node_9890);
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
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
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
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 _Noise_var = UnpackNormal(tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise)));
                float4 node_8293 = _Time;
                float node_1625 = (_WaveStrength*sin(((_Noise_var.r*3.141592654*2.0)+node_8293.g)));
                float3 node_3747 = ((node_544*max(0,dot(lightDirection,i.normalDir)))*_PosterizeMultiplier*_LightColor0.rgb*attenuation*((1.0 - _WaveStrength)+node_1625)); // Attenuate and Color
                float3 finalColor = lerp(floor(min(1.0,node_3747) * _Posterize_Number) / (_Posterize_Number - 1),node_3747,_PosterizeEffect);
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
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _WaveStrength;
            uniform float _Tessellation;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                void displacement (inout VertexInput v){
                    float node_9890 = 0.0;
                    float3 _Noise_var = UnpackNormal(tex2Dlod(_Noise,float4(TRANSFORM_TEX(v.texcoord0, _Noise),0.0,0)));
                    float4 node_8293 = _Time;
                    float node_1625 = (_WaveStrength*sin(((_Noise_var.r*3.141592654*2.0)+node_8293.g)));
                    v.vertex.xyz += float3(node_9890,node_1625,node_9890);
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
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
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
