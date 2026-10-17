using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Textures;
using Suckdex.Domain.Assets.Textures.Dxgi;
using Suckdex.Domain.Editors;
using Suckdex.Modules.TextureEditor.Views;
using Suckdex.UI.ViewModels;
using Suckdex.UI.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Suckdex.Modules.TextureEditor
{

  public class TextureEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var assetManager = containerProvider.Resolve<IAssetManager>();
      assetManager.RegisterViewTypeForExportOptionsType(typeof(DxgiTextureExportOptions), typeof(DxgiTextureAssetExportOptionsView));
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<TextureEditorView>( DefaultEditorKeys.TextureEditor );

      containerRegistry.RegisterDialog<DxgiTextureAssetExportOptionsView, AssetExportOptionsWindowViewModel<DxgiTextureExportOptions>>( nameof( DxgiTextureExportOptions ) );
    }

  }

}
