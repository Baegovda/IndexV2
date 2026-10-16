using System.Collections.ObjectModel;
using System.Windows.Data;
using Index.Modules.Logging.Logging;
using Index.UI.ViewModels;
using Prism.Events;
using Prism.Mvvm;

namespace Index.Modules.Logging.ViewModels
{

  public class LogViewModel : TabViewModelBase
  {

    #region Constants

    private const int MAX_LOG_LINES = 1000;

    #endregion

    #region Data Members

    private IEventAggregator _eventAggregator;

    private readonly object _messagesLock;
    private ObservableCollection<LogMessage> _messages;

    #endregion

    #region Properties

    public ObservableCollection<LogMessage> Messages
    {
      get => _messages;
    }

    public bool HasMessages { get; private set; }

    #endregion

    #region Constructor

    public LogViewModel( IEventAggregator eventAggregator )
    {
      TabName = "Log";

      _eventAggregator = eventAggregator;

      _messagesLock = new object();
      _messages = new ObservableCollection<LogMessage>();
      BindingOperations.EnableCollectionSynchronization( _messages, _messagesLock );

      _eventAggregator.GetEvent<LogMessageEvent>().Subscribe( OnLogMessageReceived );
    }

    #endregion

    #region Event Handlers

    private void OnLogMessageReceived( LogMessage message )
    {
      var hasMessages = false;
      lock ( _messagesLock )
      {
        _messages.Add( message );

        while ( _messages.Count > MAX_LOG_LINES )
          _messages.RemoveAt( 0 );

        hasMessages = _messages.Count > 0;
      }

      void Apply()
      {
        if ( HasMessages == hasMessages )
          return;

        HasMessages = hasMessages;
        OnPropertyChanged( nameof( HasMessages ) );
      }

      if ( Dispatcher.CheckAccess() )
        Apply();
      else
        Dispatcher.BeginInvoke( Apply );
    }

    #endregion

  }

}
