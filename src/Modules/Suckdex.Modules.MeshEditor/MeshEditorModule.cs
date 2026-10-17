using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Meshes;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Domain.Editors;
using Suckdex.Modules.MeshEditor.Views;
using Suckdex.UI.ViewModels;
using Prism.Ioc;
using Prism.Modularity;

namespace Suckdex.Modules.MeshEditor
{

  public class MeshEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var assetManager = containerProvider.Resolve<IAssetManager>();
      assetManager.RegisterViewTypeForExportOptionsType( typeof( MeshAssetExportOptions ), typeof( MeshAssetExportOptionsView ) );
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<MeshEditorView>( DefaultEditorKeys.MeshEditorKey );
      containerRegistry.RegisterDialog<MeshAssetExportOptionsView, AssetExportOptionsWindowViewModel<MeshAssetExportOptions>>( nameof( MeshAssetExportOptions ) );
    }

  }

}
