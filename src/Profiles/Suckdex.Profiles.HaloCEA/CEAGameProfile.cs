using Suckdex.Domain.FileSystem;
using Suckdex.Domain.GameProfiles;
using Suckdex.Domain.Models;
using Suckdex.Profiles.HaloCEA.FileSystem;

namespace Suckdex.Profiles.HaloCEA
{

  public class CEAGameProfile : GameProfileBase
  {

    #region Properties

    public override string GameId => "HaloCEA";
    public override string GameName => "Halo: Combat Evolved Anniversary";
    public override string Author => "Haus";

    public override IFileSystemLoader FileSystemLoader => new CEAFileSystemLoader();
    public override IGamePathIdentificationRule IdentificationRule => new CEAGamePathIdentificationRule();

    #endregion

  }

}
