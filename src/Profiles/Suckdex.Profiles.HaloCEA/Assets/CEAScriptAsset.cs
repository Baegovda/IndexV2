using Suckdex.Domain.Assets;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEAScriptAsset : CEACacheBlockEntryAsset
  {

    public override string TypeName => "Script";

    public CEAScriptAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
