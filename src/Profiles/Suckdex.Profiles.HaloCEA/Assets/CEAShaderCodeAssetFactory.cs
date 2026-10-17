using Suckdex.Domain.Assets;
using Suckdex.Domain.Jobs;
using Suckdex.Jobs;
using Prism.Ioc;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEAShaderCodeAssetFactory : AssetFactoryBase<CEAShaderCodeAsset>
  {

    public CEAShaderCodeAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    public override IJob<CEAShaderCodeAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var asset = new CEAShaderCodeAsset( assetReference );
      asset.TextStream = assetReference.Node.Open();

      return CompletedJob.FromResult( asset );
    }

  }

}
