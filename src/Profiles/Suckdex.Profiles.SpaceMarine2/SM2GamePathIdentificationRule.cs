using Suckdex.Domain.GameProfiles;

namespace Suckdex.Profiles.SpaceMarine2
{
  public class SM2GamePathIdentificationRule : IGamePathIdentificationRule
  {

    public string BaseDirectoryName => "client_pc";

    public IEnumerable<string> ChildPaths
    {
      get
      {
        yield return @"root\paks\client\resources.pak";
      }
    }

  }
}
