using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Jobs;
using Suckdex.Profiles.HaloCEA.Jobs;
using Suckdex.Textures;
using Prism.Ioc;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEATextureAssetFactory : AssetFactoryBase<DxgiTextureAsset>
  {

    #region Data Members

    private readonly IDxgiTextureService _dxgiTextureService;

    #endregion

    #region Constructor

    public CEATextureAssetFactory( IContainerProvider containerProvider )
      : base( containerProvider )
    {
      _dxgiTextureService = containerProvider.Resolve<IDxgiTextureService>();
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
