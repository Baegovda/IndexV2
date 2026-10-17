using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.HaloCEA.Assets;

namespace Suckdex.Profiles.HaloCEA.FileSystem.Files
{

  public class CEAScriptFileNode : CEACacheBlockEntryFileNode, IFileSystemAssetNode<CEAScriptAsset>
  {

    public Type AssetFactoryType => typeof( CEAScriptAssetFactory );

    public CEAScriptFileNode( IFileSystemDevice device, string name, string entryData, IFileSystemNode parent = null )
      : base( device, name, entryData, parent )
    {
    }

  }

}
