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
};

struct VertexOut {
  float4 Position: POSITION;
};

struct FragmentIn {
};

struct FragmentOut {
  float4 Color: COLOR0;
};

VertexOut Simple_vp(VertexIn v) {
  VertexOut o;

  float4 wp = mul( v.Position, worldMatrix );
  o.Position = mul( wp, viewProjMatrix );
  return o;
}

FragmentOut Simple_fp(FragmentIn f) {
  FragmentOut o;
  o.Color = float4(1,1,1,1);
  return o;
}


technique high_quality {
  pass p0 {
    AlphaBlendEnable = false;
    ZEnable = false;
    FillMode = Wireframe;
    PointSpriteEnable = false;
    VertexShader = compile vs_1_1 Simple_vp();
    PixelShader = compile ps_1_1 Simple_fp();
  }
}

