using Suckdex.Domain.Assets;
using Suckdex.Jobs;
using Suckdex.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using Prism.Ioc;

namespace Suckdex.Profiles.HaloCEA.Jobs
{

  public class DeserializeSceneJob : JobBase<SceneContext>
  {

    public DeserializeSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    protected override Task OnExecuting()
    {
      return Task.Run( () =>
      {
        SetStatus( "Deserializing Scene" );
        SetIndeterminate();

        var assetReference = Parameters.Get<IAssetReference>();
        var stream = assetReference.Node.Open();
        var reader = new NativeReader( stream, Endianness.LittleEndian );

        var scene = SaberScene.Deserialize( reader, new SerializationContext() );
        var context = SceneContext.Create( scene.Objects );

        Parameters.Set( context );
        Parameters.Set( scene );
        Parameters.Set( scene.TextureList );

        SetResult( context );
      } );
    }

  }

}
