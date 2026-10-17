using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using Prism.Regions;

namespace Suckdex.UI.ViewModels
{

  public interface IViewModel : IDisposable, INotifyPropertyChanged, INavigationAware, IConfirmNavigationRequest
  {

    #region Properties

    Dispatcher Dispatcher { get; }

    ContextMenu ContextMenu { get; }

    #endregion

  }

}
