using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.Halo2A.Assets;

namespace Suckdex.Profiles.Halo2A.FileSystem.Files
{

  public class H2ATextureDefinitionFileNode : H2AFileSystemNode, IFileSystemAssetNode<H2ATextureDefinitionAsset, H2ATextAssetFactory<H2ATextureDefinitionAsset>>
  {
    public H2ATextureDefinitionFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, startOffset, sizeInBytes, parent )
    {
    }
  }

}
