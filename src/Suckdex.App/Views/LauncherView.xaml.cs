using Suckdex.App.ViewModels;
using Suckdex.UI.Windows;

namespace Suckdex.App.Views
{

  public partial class LauncherView : IxWindow
  {

    public LauncherView( LauncherViewModel viewModel )
    {
      DataContext = viewModel;
      InitializeComponent();
    }

  }

}
