using Suckdex.Domain.Assets;
using Suckdex.Domain.FileSystem;
using Suckdex.Domain.GameProfiles;

namespace Suckdex.Domain.Models
{

  public interface IEditorEnvironment
  {

    public string GameId { get; set; }
    public string GameName { get; set; }
    public string GamePath { get; set; }
    public IGameProfile GameProfile { get; set; }

    public IAssetManager AssetManager { get; }
    public IFileSystem FileSystem { get; }

    public IParameterCollection GlobalParameters { get; }

  }

}
