using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Jobs;
using Suckdex.Profiles.Halo2A.Jobs;
using Suckdex.Textures;
using Prism.Ioc;

namespace Suckdex.Profiles.Halo2A.Assets
{

  public class H2ATextureAssetFactory : AssetFactoryBase<DxgiTextureAsset>
  {

    #region Constructor

    public H2ATextureAssetFactory( IContainerProvider containerProvider )
      : base( containerProvider )
    {
    }

    #endregion

    #region Overrides

    public override IJob<DxgiTextureAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadTextureJob>( parameters );
    }

    #endregion

  }

}
