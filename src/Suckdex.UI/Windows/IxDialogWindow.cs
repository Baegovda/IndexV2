using Prism.Services.Dialogs;

namespace Suckdex.UI.Windows
{

  public class IxDialogWindow : IxWindow, IDialogWindow
  {

    #region Properties

    public IDialogResult Result { get; set; }

    #endregion

    #region Constructor

    public IxDialogWindow()
    {
      WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
    }

    #endregion

  }

}
