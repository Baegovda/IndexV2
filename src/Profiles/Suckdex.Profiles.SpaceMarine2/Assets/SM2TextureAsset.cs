using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Jobs;
using Suckdex.Profiles.SpaceMarine2.Jobs;
using Prism.Ioc;

namespace Suckdex.Profiles.SpaceMarine2.Assets
{

  public class SM2TextureAssetFactory : AssetFactoryBase<DxgiTextureAsset>
  {
    #region Constructor

    public SM2TextureAssetFactory( IContainerProvider containerProvider )
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
