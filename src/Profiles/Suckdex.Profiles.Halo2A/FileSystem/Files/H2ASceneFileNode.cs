using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.Halo2A.Assets;

namespace Suckdex.Profiles.Halo2A.FileSystem.Files
{

  public class H2ASceneFileNode : H2AFileSystemNode, IFileSystemAssetNode<H2ASceneAsset, H2ASceneAssetFactory>
  {

    #region Properties

    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #endregion

    #region Constructor

    public H2ASceneFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, SanitizeName( name ), startOffset, sizeInBytes, parent )
    {
    }

    #endregion

  }

}
