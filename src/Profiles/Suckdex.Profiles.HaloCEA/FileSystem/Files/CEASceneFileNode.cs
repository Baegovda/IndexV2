using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.HaloCEA.Assets;

namespace Suckdex.Profiles.HaloCEA.FileSystem.Files
{

  public class CEASceneFileNode : CEAFileNode, IFileSystemAssetNode<CEASceneAsset, CEASceneAssetFactory>
  {

    #region Properties

    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #endregion

    #region Constructor

    public CEASceneFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion

  }

}
