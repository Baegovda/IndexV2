using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.Halo2A.Assets;

namespace Suckdex.Profiles.Halo2A.FileSystem.Files
{

  public class H2ATextureFileNode : H2AFileSystemNode, IFileSystemAssetNode<DxgiTextureAsset, H2ATextureAssetFactory>
  {

    #region Constructor

    public H2ATextureFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, SanitizeName( name ), startOffset, sizeInBytes, parent )
    {
    }

    #endregion

  }

}
