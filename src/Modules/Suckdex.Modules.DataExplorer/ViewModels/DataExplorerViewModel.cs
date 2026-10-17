using Suckdex.Domain.Assets;
using Suckdex.Domain.Editors;
using Suckdex.Domain.Models;
using Suckdex.Domain;
using Suckdex.Modules.DataExplorer.Services;
using Suckdex.Modules.DataExplorer.ViewModels;
using Suckdex.UI.Common;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using Suckdex.Domain.Database.Repositories;
using Suckdex.UI.Commands;
using Prism.Commands;

namespace Suckdex.Modules.DataExplorer.ViewModels;

public class DataExplorerViewModel : SearchableNodeGraphViewModel<AssetNodeViewModel>
{

  #region Data Members

  private const string FavoritesKey = "favorites";

  private readonly IAssetManager _assetManager;
  private readonly IRegionManager _regionManager;
  private readonly ISavedSettingsRepository _settingsRepository;

  #endregion

  #region Properties

  public ICommand OpenAboutDialogCommand { get; }
  public ICommand OpenBulkExportCommand { get; }
  public ObservableCollection<FavoriteItemViewModel> Favorites { get; } = new();
  public bool HasFavorites => Favorites.Count > 0;

  #endregion

  public DataExplorerViewModel( IRegionManager regionManager, IEditorEnvironment environment, ISavedSettingsRepository settingsRepository )
    : base( settingsRepository )
  {
    _regionManager = regionManager;
    _assetManager = environment.AssetManager;
    _settingsRepository = settingsRepository;

    Favorites.CollectionChanged += ( _, _ ) => OnPropertyChanged( nameof( HasFavorites ) );

    OpenAboutDialogCommand = GlobalCommands.OpenAboutDialogCommand;
    OpenBulkExportCommand = new DelegateCommand( OpenBulkExport );

    // Initialize nodes and search graph
    Initialize();
    AttachFavoriteActions();
    LoadFavorites();
  }

  protected override IxObservableCollection<AssetNodeViewModel> InitializeNodes()
  {
    var factory = new AssetNodeViewModelFactory( _assetManager );
    return new IxObservableCollection<AssetNodeViewModel>( factory.CreateNodes() );
  }

  protected override List<AssetNodeViewModel> InitializeSearchGraph( IxObservableCollection<AssetNodeViewModel> nodes )
  {
    var factory = new AssetNodeViewModelFactory( _assetManager );
    return factory.CreateNodeSearchGraph( nodes );
  }

  private void NavigateToAsset( IAssetReference assetReference )
  {
    var parameters = new NavigationParameters();
    parameters.Add( "AssetReference", assetReference );

    _regionManager.RequestNavigate( "EditorRegion", "TextureEditorView", parameters );
  }

  private void OpenBulkExport()
  {
    var parameters = new NavigationParameters();
    parameters.Add( "AssetNodes", Nodes );

    _regionManager.RequestNavigate( RegionKeys.EditorDocumentRegion, DefaultEditorKeys.BulkExport, parameters );
  }

  private void AttachFavoriteActions()
  {
    foreach ( var node in EnumerateNodes( Nodes ) )
    {
      if ( node.AssetReference is null )
        continue;

      var captured = node;
      node.QueryFavorite = () => Favorites.Any( favorite => favorite.Node == captured );
      node.RequestToggleFavorite = () => ToggleFavorite( captured );
    }
  }

  private void LoadFavorites()
  {
    var saved = _settingsRepository.GetByKey( FavoritesKey );
    if ( saved is null || string.IsNullOrWhiteSpace( saved.Data ) )
      return;

    var entries = JsonSerializer.Deserialize<List<FavoriteData>>( saved.Data );
    if ( entries is null )
      return;

    foreach ( var entry in entries )
    {
      var node = FindFavoriteNode( entry );
      if ( node?.AssetReference is null )
        continue;

      if ( Favorites.Any( favorite => favorite.Node == node ) )
        continue;

      Favorites.Add( CreateFavorite( node ) );
    }

    if ( Favorites.Count != entries.Count )
      SaveFavorites();
  }

  private void ToggleFavorite( AssetNodeViewModel node )
  {
    var existing = Favorites.FirstOrDefault( favorite => favorite.Node == node );
    if ( existing is not null )
      Favorites.Remove( existing );
    else
      Favorites.Insert( 0, CreateFavorite( node ) );

    SaveFavorites();
  }

  private void RemoveFavorite( AssetNodeViewModel node )
  {
    var existing = Favorites.FirstOrDefault( favorite => favorite.Node == node );
    if ( existing is null )
      return;

    Favorites.Remove( existing );
    SaveFavorites();
  }

  private FavoriteItemViewModel CreateFavorite( AssetNodeViewModel node )
  {
    return new FavoriteItemViewModel(
      node,
      new DelegateCommand( () => EditorCommands.OpenTabCommand.Execute( node.AssetReference ) ),
      new DelegateCommand( () => RemoveFavorite( node ) ) );
  }

  private void SaveFavorites()
  {
    var entries = Favorites
      .Where( favorite => favorite.Node.AssetReference is not null )
      .Select( favorite => new FavoriteData
      {
        AssetType = favorite.Node.AssetReference.AssetType?.FullName,
        AssetName = favorite.Node.AssetReference.AssetName,
        NodePath = favorite.Node.AssetReference.Node.GetPath()
      } )
      .ToList();

    var saved = _settingsRepository.GetByKey( FavoritesKey ) ?? _settingsRepository.New( FavoritesKey );
    saved.Data = JsonSerializer.Serialize( entries );
    _settingsRepository.Update( saved );
    _settingsRepository.SaveChanges();
  }

  private AssetNodeViewModel FindFavoriteNode( FavoriteData data )
  {
    if ( data is null )
      return null;

    AssetNodeViewModel pathMatch = null;
    var wantedType = NormalizeTypeName( data.AssetType );

    foreach ( var node in EnumerateNodes( Nodes ) )
    {
      var reference = node.AssetReference;
      if ( reference is null )
        continue;

      var sameName = string.Equals( reference.AssetName, data.AssetName, StringComparison.OrdinalIgnoreCase );
      var sameType = string.Equals( NormalizeTypeName( reference.AssetType?.FullName ), wantedType, StringComparison.Ordinal );
      if ( sameName && sameType )
        return node;

      if ( pathMatch is null
        && !string.IsNullOrWhiteSpace( data.NodePath )
        && string.Equals( reference.Node.GetPath(), data.NodePath, StringComparison.OrdinalIgnoreCase ) )
        pathMatch = node;
    }

    return pathMatch;
  }

  private static IEnumerable<AssetNodeViewModel> EnumerateNodes( IEnumerable<AssetNodeViewModel> nodes )
  {
    foreach ( var node in nodes )
    {
      yield return node;

      foreach ( var child in EnumerateNodes( node.Children ) )
        yield return child;
    }
  }

  private static string NormalizeTypeName( string typeName )
  {
    if ( string.IsNullOrWhiteSpace( typeName ) )
      return string.Empty;

    const string oldPrefix = "Index.";
    if ( typeName.StartsWith( oldPrefix, StringComparison.Ordinal ) )
      return "Suckdex." + typeName[ oldPrefix.Length.. ];

    return typeName;
  }

  private sealed class FavoriteData
  {
    public string AssetType { get; set; }
    public string AssetName { get; set; }
    public string NodePath { get; set; }
  }
}
