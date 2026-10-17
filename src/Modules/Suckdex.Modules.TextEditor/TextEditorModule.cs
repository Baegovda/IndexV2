using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Text;
using Suckdex.Domain.Editors;
using Suckdex.Modules.TextEditor.Views;
using Suckdex.UI.ViewModels;
using Suckdex.UI.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Suckdex.Modules.TextEditor
{

  public class TextEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var assetManager = containerProvider.Resolve<IAssetManager>();
      assetManager.RegisterViewTypeForExportOptionsType( typeof( TextAssetExportOptions ), typeof( AssetExportOptionsViewBase ) );
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<TextEditorView>( DefaultEditorKeys.TextEditorKey );

      containerRegistry.RegisterDialog<AssetExportOptionsViewBase, AssetExportOptionsWindowViewModel<TextAssetExportOptions>>( nameof( TextAssetExportOptions ) );
    }

  }

}
