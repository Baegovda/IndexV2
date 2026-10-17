using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Suckdex.Domain.Database.Repositories;
using Suckdex.UI.Common;
using Suckdex.UI.ViewModels;
using Prism.Commands;

namespace Suckdex.Modules.DataExplorer.ViewModels
{
  public abstract class SearchableNodeGraphViewModel<TNodeViewModel> : ViewModelBase
      where TNodeViewModel : NodeViewModelBase<TNodeViewModel>
  {
    #region Data Members

    private const string SearchHistoryKey = "search-history";
    private const int MaxSearchHistory = 20;

    private readonly ISavedSettingsRepository _settingsRepository;
    private List<TNodeViewModel> _searchGraph;

    #endregion

    #region Properties

    public IxObservableCollection<TNodeViewModel> Nodes { get; private set; }
    public ICollectionView NodesView { get; private set; }

    public ICommand SearchCommand { get; }
    public ICommand RemoveSearchCommand { get; }
    public ObservableCollection<string> SearchHistory { get; }
    public string SearchText { get; set; }

    #endregion

    #region Constructor

    protected SearchableNodeGraphViewModel( ISavedSettingsRepository settingsRepository )
    {
      _settingsRepository = settingsRepository;
      SearchHistory = new ObservableCollection<string>();
      SearchText = string.Empty;
      SearchCommand = new DelegateCommand<string>( ApplySearchTerm );
      RemoveSearchCommand = new DelegateCommand<string>( RemoveSearchTerm );
      LoadSearchHistory();
    }

    #endregion

    #region Abstract Methods

    protected abstract IxObservableCollection<TNodeViewModel> InitializeNodes();
    protected abstract List<TNodeViewModel> InitializeSearchGraph( IxObservableCollection<TNodeViewModel> nodes );

    #endregion

    #region Public Methods

    public void Initialize()
    {
      Nodes = InitializeNodes();
      _searchGraph = InitializeSearchGraph( Nodes );
      NodesView = CollectionViewSource.GetDefaultView( Nodes );

      OnInitializationComplete();

      if ( !string.IsNullOrEmpty( SearchText ) )
        ApplySearchTerm( SearchText );
    }

    #endregion

    #region Private Methods

    private async void ApplySearchTerm( string searchTerm )
    {
      Application.Current.Dispatcher.Invoke( () => RememberSearchTerm( searchTerm ) );

      await Task.Run( () =>
      {
        Nodes.SuppressNotifications = true;

        bool isEmpty = string.IsNullOrEmpty( searchTerm );
        bool changesMade = false;

        foreach ( var node in _searchGraph )
          node.Children.SuppressNotifications = true;

        foreach ( var node in _searchGraph )
        {
          bool wasVisible = node.IsVisible;
          if ( isEmpty )
          {
            node.IsVisible = true;
          }
          else
          {
            node.IsVisible = node.Name.Contains( searchTerm, StringComparison.OrdinalIgnoreCase ) ||
                             node.Children.Any( x => x.IsVisible );
          }

          if ( node.IsVisible != wasVisible )
          {
            changesMade = true;
          }
        }

        foreach ( var node in _searchGraph )
          node.Children.SuppressNotifications = false;

        Nodes.SuppressNotifications = false;

        Application.Current.Dispatcher.BeginInvoke( () =>
        {
          foreach ( var node in _searchGraph )
            if ( !node.IsLeaf )
              node.ChildrenView.Refresh();
        } );
      } );
    }

    protected virtual void OnInitializationComplete()
    {
    }

    private void LoadSearchHistory()
    {
      var saved = _settingsRepository.GetByKey( SearchHistoryKey );
      if ( saved is null || string.IsNullOrWhiteSpace( saved.Data ) )
        return;

      var history = JsonSerializer.Deserialize<SearchHistoryData>( saved.Data );
      if ( history is null )
        return;

      foreach ( var term in history.Terms.Where( term => !string.IsNullOrWhiteSpace( term ) ) )
        SearchHistory.Add( term );

      SearchText = history.Current ?? string.Empty;
    }

    private void RememberSearchTerm( string searchTerm )
    {
      searchTerm ??= string.Empty;
      var terms = SearchHistory
        .Where( term => !string.Equals( term, searchTerm, StringComparison.OrdinalIgnoreCase ) )
        .ToList();

      if ( !string.IsNullOrWhiteSpace( searchTerm ) )
        terms.Insert( 0, searchTerm );

      if ( terms.Count > MaxSearchHistory )
        terms = terms.Take( MaxSearchHistory ).ToList();

      SaveSearchHistory( terms, searchTerm );
    }

    private void RemoveSearchTerm( string term )
    {
      if ( string.IsNullOrWhiteSpace( term ) )
        return;

      var terms = SearchHistory
        .Where( existing => !string.Equals( existing, term, StringComparison.OrdinalIgnoreCase ) )
        .ToList();

      SaveSearchHistory( terms, SearchText ?? string.Empty );
    }

    private void SaveSearchHistory( IReadOnlyList<string> terms, string current )
    {
      SearchHistory.Clear();
      foreach ( var term in terms )
        SearchHistory.Add( term );

      var saved = _settingsRepository.GetByKey( SearchHistoryKey );
      if ( saved is null )
        saved = _settingsRepository.New( SearchHistoryKey );

      saved.Data = JsonSerializer.Serialize( new SearchHistoryData
      {
        Terms = terms.ToList(),
        Current = current
      } );
      _settingsRepository.Update( saved );
      _settingsRepository.SaveChanges();
    }

    private sealed class SearchHistoryData
    {

      public List<string> Terms { get; set; } = new();
      public string Current { get; set; } = string.Empty;

    }

    #endregion
  }
}
