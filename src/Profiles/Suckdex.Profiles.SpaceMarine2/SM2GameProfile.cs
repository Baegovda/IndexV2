using Suckdex.Domain.FileSystem;
using Suckdex.Domain.GameProfiles;
using Suckdex.Profiles.SpaceMarine2.FileSystem;

namespace Suckdex.Profiles.SpaceMarine2
{
  public class SM2GameProfile : GameProfileBase
  {
    #region Properties

    public override string GameId => "SpaceMarine2";
    public override string GameName => "Warhammer 40k: Space Marine 2";
    public override string Author => "Haus";

    public override IFileSystemLoader FileSystemLoader => new SM2FileSystemLoader();
    public override IGamePathIdentificationRule IdentificationRule => new SM2GamePathIdentificationRule();

    #endregion
  }
}
