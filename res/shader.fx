shared float4x4 worldMatrix;
shared float4x4 viewProjMatrix;
shared texture diffuseTexture;

sampler s_DiffuseTexture = sampler_state {
  Texture = <diffuseTexture>;
  MinFilter = Linear;
  MagFilter = Linear;
  MipFilter = Linear;

  AddressU = Wrap;
  AddressV = Wrap;
  AddressW = Wrap;
};

struct VertexIn {
  float4 Position: POSITION;
  float3 Normal: NORMAL;
  float2 Tex0: TEXCOORD0;
};

struct VertexOut {
  float4 Position: POSITION;
  float2 Tex0: TEXCOORD0;
  float intensity: TEXCOORD1;
};

struct FragmentIn {
  float2 Tex0: TEXCOORD0;
  float intensity: TEXCOORD1;
};

struct FragmentOut {
  float4 Color: COLOR0;
};

VertexOut Simple_vp(VertexIn v) {
  VertexOut o;

  float4 wp = mul( v.Position, worldMatrix );
  o.Position = mul( wp, viewProjMatrix );
  o.Tex0 = v.Tex0;
  
  o.intensity = mul( float4(v.Normal, 1), worldMatrix ).y + 1 * 0.5;
    
  return o;
}

FragmentOut Simple_fp(FragmentIn f) {
  FragmentOut o;
  o.Color = tex2D(s_DiffuseTexture, f.Tex0) * f.intensity;
  return o;
}


technique high_quality {
  pass p0 {
    AlphaBlendEnable = false;
    ZWriteEnable = true;
    CullMode = Cw;
    VertexShader = compile vs_1_1 Simple_vp();
    PixelShader = compile ps_1_1 Simple_fp();
  }
}

