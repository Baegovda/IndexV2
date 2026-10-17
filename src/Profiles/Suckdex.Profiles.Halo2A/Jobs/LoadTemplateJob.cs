using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures;
using Suckdex.Jobs;
using Suckdex.Profiles.Halo2A.Assets;
using Suckdex.Profiles.Halo2A.Meshes;
using Prism.Ioc;

namespace Suckdex.Profiles.Halo2A.Jobs
{

  public class LoadTemplateJob : CompositeJobBase<H2ATemplateAsset>
  {

    #region Constructor

    public LoadTemplateJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      var assetReference = Parameters.Get<IAssetReference>();
      Name = $"Loading Template {assetReference.AssetName}";
      SetStatus( Name );

      Parameters.Set<IAssetLoadContext>( new AssetLoadContext() );
    }

    #endregion

    #region Overrides

    protected override void CreateSubJobs()
    {
      AddJob<DeserializeTemplateJob>();
      AddJob<ConvertGeometryJob>();
      AddJob<LoadGeometryTexturesJob>();
      AddJob<IdentifyMeshesJob>();
    }

    protected override async Task OnCompleted()
    {
      var assetReference = Parameters.Get<IAssetReference>();
      var asset = new H2ATemplateAsset( assetReference );
      asset.AssetLoadContext = Parameters.Get<IAssetLoadContext>();
      asset.AssimpScene = Parameters.Get<SceneContext>().Scene;
      asset.Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
      asset.LodMeshNames = Parameters.Get<ISet<string>>( "LodMeshSet" );
      asset.VolumeMeshNames = Parameters.Get<ISet<string>>( "VolumeMeshSet" );

      SetResult( asset );
    }

    #endregion

  }

}
