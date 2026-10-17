using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Text;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public abstract class CEACacheBlockEntryAsset : TextAsset
  {

    public CEACacheBlockEntryAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
