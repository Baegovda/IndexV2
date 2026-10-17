using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.HaloCEA.Assets;

namespace Suckdex.Profiles.HaloCEA.FileSystem.Files
{

  public class CEAShaderCodeFileNode : CEAFileNode, IFileSystemAssetNode<CEAShaderCodeAsset>
  {

    public Type AssetFactoryType => typeof( CEAShaderCodeAssetFactory );

    public CEAShaderCodeFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

  }

}
