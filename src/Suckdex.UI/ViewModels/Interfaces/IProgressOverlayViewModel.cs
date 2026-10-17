using Suckdex.Common;

namespace Suckdex.UI.ViewModels
{

  public interface IProgressOverlayViewModel : IViewModel
  {

    #region Properties

    public bool ShowProgressOverlay { get; }
    public IProgressInfo Progress { get; }

    #endregion

  }

}
