using Suckdex.Domain.Assets;
using Suckdex.Domain.Jobs;
using Suckdex.Jobs;
using Prism.Ioc;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEACacheBlockEntryAssetFactory<TAsset> : AssetFactoryBase<TAsset>
    where TAsset : CEACacheBlockEntryAsset
  {

    #region Constructor

    public CEACacheBlockEntryAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<TAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var asset = CreateAsset( assetReference );
      asset.TextStream = assetReference.Node.Open();

      return CompletedJob.FromResult( asset );
    }

    #endregion

    #region Private Methods

    private TAsset CreateAsset( IAssetReference assetReference )
      => ( TAsset ) Activator.CreateInstance( typeof( TAsset ), new object[] { assetReference } );

    #endregion

  }

}
