using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Meshes;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.HaloCEA.Assets;

namespace Suckdex.Profiles.HaloCEA.FileSystem.Files
{

  public class CEATemplateFileNode : CEAFileNode, IFileSystemAssetNode<CEATemplateAsset, CEATemplateAssetFactory>
  {


    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #region Constructor

    public CEATemplateFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion

  }

}
