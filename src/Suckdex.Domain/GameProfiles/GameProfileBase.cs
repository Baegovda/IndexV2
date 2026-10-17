using Suckdex.Domain.FileSystem;
using Suckdex.Domain.Models;

namespace Suckdex.Domain.GameProfiles
{

  public abstract class GameProfileBase : IGameProfile
  {

    #region Properties

    public abstract string GameId { get; }
    public abstract string GameName { get; }
    public abstract string Author { get; }
    public virtual Version Version => DefaultGetVersion();

    public abstract IFileSystemLoader FileSystemLoader { get; }
    public abstract IGamePathIdentificationRule IdentificationRule { get; }

    #endregion

    #region Public Methods

    public virtual Stream? LoadGameIcon()
    {
      const string iconName = "GameIcon";
      var assembly = GetType().Assembly;

      var gameIconResourceName = assembly.GetManifestResourceNames()
        .FirstOrDefault( resourceName =>
        {
          var fileName = Path.GetFileNameWithoutExtension( resourceName );
          var shortName = fileName.Split( '.' ).LastOrDefault();
          return string.Equals( shortName, iconName, StringComparison.OrdinalIgnoreCase );
        } );

      if ( gameIconResourceName is null )
        return null;

      return assembly.GetManifestResourceStream( gameIconResourceName );
    }

    #endregion

    #region Virtual Methods

    public virtual Task Initialize( IEditorEnvironment environment )
      => Task.CompletedTask;

    #endregion

    #region Private Methods

    private Version DefaultGetVersion()
    {
      var assembly = GetType().Assembly;
      var version = assembly.GetName().Version;
      return version ?? new Version( 0, 0 );
    }

    #endregion

  }

}
