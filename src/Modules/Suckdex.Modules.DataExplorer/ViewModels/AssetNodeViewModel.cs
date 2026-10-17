using System;
using Suckdex.Domain.Assets;
using Suckdex.UI.Commands;
using Suckdex.UI.Controls.Menus;
using Prism.Commands;

namespace Suckdex.Modules.DataExplorer.ViewModels
{

  public class AssetNodeViewModel : NodeViewModelBase<AssetNodeViewModel>
  {

    #region Data Members

    private readonly string _name;
    private readonly IAssetReference _assetReference;

    #endregion

    #region Properties

    public override string Name
    {
      get => _name;
    }

    internal IAssetReference AssetReference
    {
      get => _assetReference;
    }

    public Type AssetType
    {
      get;
      set;
    }

    internal Func<bool> QueryFavorite { get; set; }

    internal Action RequestToggleFavorite { get; set; }

    #endregion

    #region Constructor

    public AssetNodeViewModel( IAssetReference assetReference )
    {
      _assetReference = assetReference;
      _name = assetReference.Node.DisplayName;

      DoubleClickCommand = new DelegateCommand( HandleDoubleClick );
    }

    public AssetNodeViewModel( string assetTypeName )
    {
      _name = assetTypeName;
    }

    #endregion

    #region Overrides

    protected override void OnConfigureContextMenu( MenuViewModelBuilder builder )
    {
      if ( _assetReference is null )
        return;

      builder
        .AddItem( "Open", item =>
        {
          item.Command( EditorCommands.OpenTabCommand, _assetReference );
        } );

      if ( RequestToggleFavorite is null )
        return;

      var header = QueryFavorite?.Invoke() == true
        ? "Remove from Favorites"
        : "Add to Favorites";

      builder.AddItem( header, item =>
      {
        item.Command( new DelegateCommand( RequestToggleFavorite ) );
      } );
    }

    #endregion

    #region Private Methods

    private void HandleDoubleClick()
      => EditorCommands.OpenTabCommand.Execute( _assetReference );

    #endregion

  }

}
