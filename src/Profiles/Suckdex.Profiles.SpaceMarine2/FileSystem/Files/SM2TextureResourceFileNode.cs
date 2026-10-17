using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Domain.FileSystem;
using Suckdex.Profiles.SpaceMarine2.Assets;
using LibSaber.SpaceMarine2.Structures.Resources;

namespace Suckdex.Profiles.SpaceMarine2.FileSystem.Files
{

  public class SM2TextureResourceFileNode : SM2ResourceFileNode<resDESC_PCT>, IFileSystemAssetNode<DxgiTextureAsset, SM2TextureAssetFactory>
  {

    #region Constructor

    public SM2TextureResourceFileNode( IFileSystemDevice device, fioZIP_CACHE_FILE.ENTRY entry, IFileSystemNode parent = null )
      : base( device, entry, parent )
    {
    }

    #endregion

  }

}
