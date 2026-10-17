using ICSharpCode.AvalonEdit.Search;
using Suckdex.Modules.TextEditor.AvalonEdit;
using Suckdex.UI.Views;

namespace Suckdex.Modules.TextEditor.Views
{

  public partial class TextEditorView : EditorView
  {

    #region Constructor

    public TextEditorView()
    {
      InitializeComponent();
      SearchPanel.Install( TextEditor.TextArea );
      HighlightCurrentLineBackgroundRenderer.Install( TextEditor );
    }

    #endregion



  }

}
