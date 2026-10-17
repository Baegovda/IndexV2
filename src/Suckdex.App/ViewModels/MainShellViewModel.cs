using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Suckdex.App.Views;
using Suckdex.Domain;
using Suckdex.Domain.Assets;
using Suckdex.Domain.Models;
using Suckdex.Modules.JobManager.Views;
using Suckdex.UI.Commands;
using Suckdex.UI.ViewModels;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Serilog;

namespace Suckdex.App.ViewModels
{

  public class MainShellViewModel : WindowViewModel
  {

    #region Properties

    protected IEditorEnvironment EditorEnvironment { get; }

    public DelegateCommand ShowLauncherCommand { get; }

    #endregion

    #region Data Members

    private bool _discardTabsOnClose;

    #endregion

    #region Constructor

    public MainShellViewModel( IContainerProvider container, IEditorEnvironment environment )
      : base( container )
    {
      Title = $"Suckdex | {environment.GameName}";
      EditorEnvironment = environment;
      ShowLauncherCommand = new DelegateCommand( ShowLauncher );
    }

    #endregion

    #region Overrides

    protected override void OnWindowAppeared()
    {
      InitializeBottomTabPanel();
      Log.Logger.Information( "Welcome to Suckdex! ^_^" );
      Log.Logger.Information( "Using Profile '{ProfileName:l}' v{VersionName:l} by {Author:l}",
        EditorEnvironment.GameName,
        EditorEnvironment.GameProfile.Version,
        EditorEnvironment.GameProfile.Author );

      Dispatcher.CurrentDispatcher.BeginInvoke( RestoreTabs, DispatcherPriority.ApplicationIdle );

      //var assetManager = Container.Resolve<IAssetManager>();
      //assetManager.TryGetAssetReference( typeof( IMeshAsset ), "a10/a10.lg", out var modelRef );
      //EditorCommands.NavigateToAssetCommand.Execute( modelRef );
      //assetManager.TryGetAssetReference( typeof( IMeshAsset ), "a10/cryotube_a10__h", out modelRef );
      //EditorCommands.NavigateToAssetCommand.Execute( modelRef );
      //assetManager.TryGetAssetReference( typeof( IMeshAsset ), "a10/cyborg", out modelRef );
      //EditorCommands.NavigateToAssetCommand.Execute( modelRef );
    }

    protected override void OnWindowClosing( CancelEventArgs e )
    {
      SaveSession( !_discardTabsOnClose );
      EditorCommands.CloseAllTabsCommand.Execute( null );
    }

    #endregion

    #region Private Methods

    private void ShowLauncher()
    {
      var previousGameId = EditorEnvironment.GameId;
      var previousGamePath = EditorEnvironment.GamePath;
      var launcher = Container.Resolve<LauncherView>();
      launcher.Owner = Window;

      if ( launcher.ShowDialog() != true )
        return;

      if ( EditorEnvironment.GameId == previousGameId && EditorEnvironment.GamePath == previousGamePath )
        return;

      _discardTabsOnClose = true;
      SaveSession( false );

      Process.Start( new ProcessStartInfo( Environment.ProcessPath )
      {
        UseShellExecute = true
      } );
      Application.Current.Shutdown();
    }

    private void RestoreTabs()
    {
      var session = EditorSessionStore.Load();
      if ( session?.Tabs is null || session.Tabs.Count == 0 )
        return;

      if ( !string.Equals( session.GameId, EditorEnvironment.GameId, StringComparison.OrdinalIgnoreCase )
        || !string.Equals( session.GamePath, EditorEnvironment.GamePath, StringComparison.OrdinalIgnoreCase ) )
        return;

      foreach ( var tab in session.Tabs )
      {
        var assetReference = FindAsset( tab );
        if ( assetReference is null )
        {
          Log.Warning( "Could not restore asset {AssetName}", tab.AssetName );
          continue;
        }

        EditorCommands.OpenTabCommand.Execute( assetReference );
      }
    }

    private IAssetReference FindAsset( EditorTabSession tab )
    {
      IAssetReference nameMatch = null;

      foreach ( var reference in EditorEnvironment.AssetManager.ReferenceCollections.SelectMany( collection => collection ) )
      {
        if ( string.Equals( reference.AssetName, tab.AssetName, StringComparison.OrdinalIgnoreCase ) )
        {
          if ( SameAssetType( reference.AssetType.FullName, tab.AssetType ) )
            return reference;

          nameMatch ??= reference;
          continue;
        }

        if ( !string.IsNullOrWhiteSpace( tab.NodePath )
          && string.Equals( reference.Node.GetPath(), tab.NodePath, StringComparison.OrdinalIgnoreCase ) )
          return reference;
      }

      return nameMatch;
    }

    private static bool SameAssetType( string current, string saved )
    {
      if ( string.Equals( current, saved, StringComparison.Ordinal ) )
        return true;

      return string.Equals( ToSuckdexTypeName( current ), ToSuckdexTypeName( saved ), StringComparison.Ordinal );
    }

    private static string ToSuckdexTypeName( string typeName )
    {
      const string oldPrefix = "Index.";
      if ( typeName is not null && typeName.StartsWith( oldPrefix, StringComparison.Ordinal ) )
        return "Suckdex." + typeName.Substring( oldPrefix.Length );

      return typeName;
    }

    private void SaveSession( bool keepTabs )
    {
      var session = EditorSessionStore.Load() ?? new EditorSession();
      session.GameId = EditorEnvironment.GameId;
      session.GamePath = EditorEnvironment.GamePath;
      session.Tabs = keepTabs ? CaptureTabs() : new List<EditorTabSession>();

      if ( Window is not null )
      {
        var bounds = Window.WindowState == WindowState.Normal
          ? new Rect( Window.Left, Window.Top, Window.Width, Window.Height )
          : Window.RestoreBounds;

        session.Left = bounds.Left;
        session.Top = bounds.Top;
        session.Width = bounds.Width;
        session.Height = bounds.Height;
        session.Maximized = Window.WindowState == WindowState.Maximized;
      }

      EditorSessionStore.Save( session );
    }

    private List<EditorTabSession> CaptureTabs()
    {
      if ( !RegionManager.Regions.ContainsRegionWithName( RegionKeys.EditorDocumentRegion ) )
        return new List<EditorTabSession>();

      var region = RegionManager.Regions[ RegionKeys.EditorDocumentRegion ];
      return region.Views
        .OfType<FrameworkElement>()
        .Select( view => view.DataContext )
        .OfType<IEditorViewModel>()
        .Where( viewModel => viewModel.AssetReference is not null )
        .Select( viewModel => new EditorTabSession
        {
          AssetType = viewModel.AssetReference.AssetType.FullName ?? string.Empty,
          AssetName = viewModel.AssetReference.AssetName,
          NodePath = viewModel.AssetReference.Node.GetPath()
        } )
        .ToList();
    }

    private void InitializeBottomTabPanel()
    {
      RegionManager.RequestNavigate( RegionKeys.BottomPanelRegion, "LogView" );

      var region = RegionManager.Regions[ RegionKeys.BottomPanelRegion ];
      region.Add( Container.Resolve<JobsView>() );
    }

    #endregion

  }

}
