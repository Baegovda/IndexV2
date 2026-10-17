using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using Suckdex.Domain.Database;
using Suckdex.Domain.Database.Entities;

namespace Suckdex.App
{

  internal sealed class EditorSession
  {

    public string GameId { get; set; } = string.Empty;
    public string GamePath { get; set; } = string.Empty;
    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public bool Maximized { get; set; }
    public List<EditorTabSession> Tabs { get; set; } = new();

  }

  internal sealed class EditorTabSession
  {

    public string AssetType { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
    public string NodePath { get; set; } = string.Empty;

  }

  internal static class EditorSessionStore
  {

    private const string SessionGameId = "";
    private const string SessionKey = "editor-session";

    public static EditorSession? Load()
    {
      using var context = new IndexDataContext();
      var row = context.Set<SavedSettings>()
        .FirstOrDefault( x => x.GameId == SessionGameId && x.Key == SessionKey );

      if ( row is null || string.IsNullOrWhiteSpace( row.Data ) )
        return null;

      return JsonSerializer.Deserialize<EditorSession>( row.Data );
    }

    public static void Save( EditorSession session )
    {
      using var context = new IndexDataContext();
      var set = context.Set<SavedSettings>();
      var row = set.FirstOrDefault( x => x.GameId == SessionGameId && x.Key == SessionKey );
      var data = JsonSerializer.Serialize( session );

      if ( row is null )
      {
        set.Add( new SavedSettings
        {
          GameId = SessionGameId,
          Key = SessionKey,
          Data = data
        } );
      }
      else
      {
        row.Data = data;
      }

      context.SaveChanges();
    }

    public static void RememberGame( string gameId, string gamePath )
    {
      var session = Load() ?? new EditorSession();
      if ( session.GameId != gameId || session.GamePath != gamePath )
        session.Tabs = new List<EditorTabSession>();

      session.GameId = gameId;
      session.GamePath = gamePath;
      Save( session );
    }

    public static bool TryGetRestorableSession( out EditorSession? session )
    {
      session = Load();
      if ( IsRestorable( session ) )
        return true;

      session = TrySingleKnownGame();
      return session is not null;
    }

    private static bool IsRestorable( EditorSession? session )
    {
      if ( session is null || string.IsNullOrWhiteSpace( session.GameId ) || string.IsNullOrWhiteSpace( session.GamePath ) )
        return false;

      if ( !Directory.Exists( session.GamePath ) )
        return false;

      return IsKnownPath( session.GameId, session.GamePath );
    }

    private static EditorSession? TrySingleKnownGame()
    {
      using var context = new IndexDataContext();
      var paths = context.Set<GamePath>().ToList();
      if ( paths.Count != 1 || !Directory.Exists( paths[ 0 ].Path ) )
        return null;

      return new EditorSession
      {
        GameId = paths[ 0 ].GameId,
        GamePath = paths[ 0 ].Path
      };
    }

    private static bool IsKnownPath( string gameId, string gamePath )
    {
      using var context = new IndexDataContext();
      return context.Set<GamePath>().Any( x => x.GameId == gameId && x.Path == gamePath );
    }

    public static void ApplyWindow( Window window )
    {
      var session = Load();
      if ( session is null || session.Width < 100 || session.Height < 100 )
        return;

      if ( !IsOnScreen( session ) )
        return;

      window.WindowStartupLocation = WindowStartupLocation.Manual;
      window.Left = session.Left;
      window.Top = session.Top;
      window.Width = session.Width;
      window.Height = session.Height;

      if ( session.Maximized )
        window.WindowState = WindowState.Maximized;
    }

    private static bool IsOnScreen( EditorSession session )
    {
      var left = SystemParameters.VirtualScreenLeft;
      var top = SystemParameters.VirtualScreenTop;
      var right = left + SystemParameters.VirtualScreenWidth;
      var bottom = top + SystemParameters.VirtualScreenHeight;

      return session.Left < right - 80
        && session.Left + session.Width > left + 80
        && session.Top < bottom - 40
        && session.Top + session.Height > top + 40;
    }

  }

}
