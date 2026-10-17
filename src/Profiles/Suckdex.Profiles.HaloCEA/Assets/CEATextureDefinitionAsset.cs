using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Text;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEATextureDefinitionAsset : CEACacheBlockEntryAsset
  {

    public override string TypeName => "Texture Definition";

    public CEATextureDefinitionAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
