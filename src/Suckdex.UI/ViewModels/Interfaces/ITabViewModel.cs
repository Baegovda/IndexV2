using System.Windows.Input;

namespace Suckdex.UI.ViewModels
{

  public interface ITabViewModel : IViewModel
  {

    #region Properties

    public string TabName { get; set; }
    public ICommand CloseCommand { get; }

    #endregion

  }

}
