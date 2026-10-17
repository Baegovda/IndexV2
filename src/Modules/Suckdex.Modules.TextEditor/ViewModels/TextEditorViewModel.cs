using System.IO;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Suckdex.Domain.Assets.Text;
using Suckdex.UI.ViewModels;
using Prism.Ioc;

namespace Suckdex.Modules.TextEditor.ViewModels
{

  public class TextEditorViewModel : EditorViewModelBase<ITextAsset>
  {

    #region Properties

    public TextDocument Document { get; set; }

    #endregion

    #region Constructor

    public TextEditorViewModel( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    protected override void OnAssetLoaded( ITextAsset asset )
    {
      base.OnAssetLoaded( asset );

      // TODO: Streaming text instead of string
      string documentText = string.Empty;
      using ( var reader = new StreamReader( asset.TextStream, leaveOpen: true ) )
        documentText = reader.ReadToEnd();

      Dispatcher.Invoke( () =>
      {
        Document = new TextDocument( documentText );
      } );
    }

    #endregion

  }

}
