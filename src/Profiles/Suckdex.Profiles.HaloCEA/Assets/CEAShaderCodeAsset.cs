using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Text;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEAShaderCodeAsset : TextAsset
  {

    public override string TypeName => "Shader Code";

    public CEAShaderCodeAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
