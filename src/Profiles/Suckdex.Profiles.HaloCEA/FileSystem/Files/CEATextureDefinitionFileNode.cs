using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.HaloCEA.Assets;

namespace Suckdex.Profiles.HaloCEA.FileSystem.Files
{

  public class CEATextureDefinitionFileNode : CEACacheBlockEntryFileNode, IFileSystemAssetNode<CEATextureDefinitionAsset>
  {

    public Type AssetFactoryType => typeof( CEATextureDefinitionAssetFactory );

    public CEATextureDefinitionFileNode( IFileSystemDevice device, string name, string entryData, IFileSystemNode parent = null )
      : base( device, name, entryData, parent )
    {
    }

  }

}
