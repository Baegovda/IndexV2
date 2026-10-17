using Suckdex.Domain.Assets;
using Suckdex.Domain.Jobs;
using Suckdex.Jobs;
using Prism.Ioc;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEAScriptAssetFactory : CEACacheBlockEntryAssetFactory<CEAScriptAsset>
  {

    #region Constructor

    public CEAScriptAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

  }

}
