using Suckdex.Domain.FileSystem;
using Suckdex.Domain.GameProfiles;
using Suckdex.Profiles.Halo2A.FileSystem;

namespace Suckdex.Profiles.Halo2A
{

  public class H2AGameProfile : GameProfileBase
  {

    #region Properties

    public override string GameId => "Halo2A";
    public override string GameName => "Halo 2: Anniversary";
    public override string Author => "Haus";

    public override IFileSystemLoader FileSystemLoader => new H2AFileSystemLoader();
    public override IGamePathIdentificationRule IdentificationRule => new H2AGamePathIdentificationRule();

    #endregion

  }

}
