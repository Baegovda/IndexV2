using System.Windows.Controls;
using Suckdex.UI.Views;

namespace Suckdex.Modules.MeshEditor.Views
{

  public partial class MeshEditorView : EditorView
  {

    #region Constructor

    public MeshEditorView()
    {
      InitializeComponent();
    }

    #endregion

    #region Overrides

    protected override void OnDisposing( bool dispose )
    {
      base.OnDisposing( dispose );

      Dispatcher.Invoke( () =>
      {
        if ( this.Content is Panel contentPanel )
          contentPanel.Children.Clear();
      } );

      MeshViewer?.Dispose();
      MeshViewer = null;
    }

    #endregion

  }

}
