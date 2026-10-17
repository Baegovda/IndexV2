using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Suckdex.Modules.DataExplorer.ViewModels;

namespace Suckdex.Modules.DataExplorer.Views
{
    /// <summary>
    /// Interaction logic for DataExplorerView.xaml
    /// </summary>
    public partial class DataExplorerView : UserControl
    {
        private bool _openingNodeMenu;

        public DataExplorerView()
        {
            InitializeComponent();
        }

        private void OnAssetClick( object sender, MouseButtonEventArgs e )
        {
            if ( sender is not TreeViewItem item || e.OriginalSource is not DependencyObject source )
                return;

            if ( FindAncestor<TreeViewItem>( source ) != item )
                return;

            if ( item.DataContext is not AssetNodeViewModel node || node.DoubleClickCommand is null )
                return;

            if ( !node.DoubleClickCommand.CanExecute( null ) )
                return;

            node.DoubleClickCommand.Execute( null );
        }

        private void OnAssetSelected( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            if ( !IsArrowSelecting() )
                return;

            OpenAsset( e.NewValue as AssetNodeViewModel );
        }

        private void OnFavoriteSelected( object sender, SelectionChangedEventArgs e )
        {
            if ( !IsArrowSelecting() )
                return;

            if ( sender is ListBox list )
                OpenFavorite( list.SelectedItem as FavoriteItemViewModel );
        }

        private static bool IsArrowSelecting()
            => Keyboard.IsKeyDown( Key.Up ) || Keyboard.IsKeyDown( Key.Down );

        private static void OpenAsset( AssetNodeViewModel node )
        {
            if ( node?.DoubleClickCommand is null || !node.DoubleClickCommand.CanExecute( null ) )
                return;

            node.DoubleClickCommand.Execute( null );
        }

        private static void OpenFavorite( FavoriteItemViewModel favorite )
        {
            if ( favorite?.OpenCommand is null || !favorite.OpenCommand.CanExecute( null ) )
                return;

            favorite.OpenCommand.Execute( null );
        }

        private void OnFavoriteClick( object sender, MouseButtonEventArgs e )
        {
            if ( sender is not ListBoxItem item || e.OriginalSource is not DependencyObject source )
                return;

            if ( FindAncestor<Button>( source ) is not null )
                return;

            if ( item.DataContext is not FavoriteItemViewModel favorite || favorite.OpenCommand is null )
                return;

            if ( !favorite.OpenCommand.CanExecute( null ) )
                return;

            favorite.OpenCommand.Execute( null );
        }

        private static T FindAncestor<T>( DependencyObject source )
            where T : DependencyObject
        {
            while ( source is not null )
            {
                if ( source is T match )
                    return match;

                source = VisualTreeHelper.GetParent( source );
            }

            return null;
        }

        private void OnNodeContextMenuOpening( object sender, ContextMenuEventArgs e )
        {
            if ( _openingNodeMenu || sender is not TreeViewItem item )
                return;

            if ( e.OriginalSource is not DependencyObject source || FindAncestor<TreeViewItem>( source ) != item )
                return;

            if ( item.DataContext is not AssetNodeViewModel node || node.AssetReference is null )
            {
                e.Handled = true;
                return;
            }

            var menu = new ContextMenu();
            var openItem = new MenuItem { Header = "Open" };
            openItem.Click += ( _, _ ) => OpenAsset( node );
            menu.Items.Add( openItem );

            if ( node.RequestToggleFavorite is not null )
            {
                var favoriteItem = new MenuItem
                {
                    Header = node.QueryFavorite?.Invoke() == true
                        ? "Remove from Favorites"
                        : "Add to Favorites"
                };
                favoriteItem.Click += ( _, _ ) => node.RequestToggleFavorite();
                menu.Items.Add( favoriteItem );
            }

            item.ContextMenu = menu;
            menu.PlacementTarget = item;
            e.Handled = true;

            item.Dispatcher.BeginInvoke( () =>
            {
                if ( !ReferenceEquals( item.ContextMenu, menu ) )
                    return;

                _openingNodeMenu = true;
                menu.IsOpen = true;
                _openingNodeMenu = false;
            } );
        }
    }
}
