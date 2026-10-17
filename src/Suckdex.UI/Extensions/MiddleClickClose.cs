using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Suckdex.UI.ViewModels;

namespace Suckdex.UI.Extensions
{

  public static class MiddleClickClose
  {

    private static readonly TimeSpan HoldDelay = TimeSpan.FromMilliseconds( 320 );
    private static readonly TimeSpan HoldInterval = TimeSpan.FromMilliseconds( 130 );
    private const double DragDistance = 8;

    private static DispatcherTimer _holdTimer;
    private static TabControl _holdTabControl;
    private static Point _holdStart;
    private static TabItem _lastClosed;
    private static bool _endingHold;

    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
      "IsEnabled",
      typeof( bool ),
      typeof( MiddleClickClose ),
      new PropertyMetadata( false, OnIsEnabledChanged ) );

    public static void SetIsEnabled( DependencyObject element, bool value )
      => element.SetValue( IsEnabledProperty, value );

    public static bool GetIsEnabled( DependencyObject element )
      => ( bool ) element.GetValue( IsEnabledProperty );

    private static void OnIsEnabledChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
    {
      if ( sender is not TabItem tabItem )
        return;

      tabItem.PreviewMouseDown -= OnPreviewMouseDown;
      if ( GetIsEnabled( tabItem ) )
        tabItem.PreviewMouseDown += OnPreviewMouseDown;
    }

    private static void OnPreviewMouseDown( object sender, MouseButtonEventArgs e )
    {
      if ( e.ChangedButton != MouseButton.Middle )
        return;

      if ( sender is not TabItem tabItem )
        return;

      var tabControl = FindAncestor<TabControl>( tabItem );
      if ( !Close( tabItem ) )
        return;

      e.Handled = true;
      if ( tabControl is null )
        return;

      BeginHold( tabControl, e.GetPosition( tabControl ) );
    }

    private static void BeginHold( TabControl tabControl, Point start )
    {
      EndHold();

      _holdTabControl = tabControl;
      _holdStart = start;
      _lastClosed = null;

      tabControl.PreviewMouseMove += OnHoldMouseMove;
      tabControl.PreviewMouseUp += OnHoldMouseUp;
      tabControl.LostMouseCapture += OnHoldLostCapture;
      tabControl.CaptureMouse();

      _holdTimer = new DispatcherTimer( DispatcherPriority.Input, tabControl.Dispatcher )
      {
        Interval = HoldDelay
      };
      _holdTimer.Tick += OnHoldTick;
      _holdTimer.Start();
    }

    private static void OnHoldTick( object sender, EventArgs e )
    {
      if ( _holdTabControl is null || Mouse.MiddleButton != MouseButtonState.Pressed )
      {
        EndHold();
        return;
      }

      if ( Mouse.Captured != _holdTabControl )
        _holdTabControl.CaptureMouse();

      CloseUnderCursor( _holdTabControl );

      if ( _holdTimer is not null && _holdTimer.Interval != HoldInterval )
        _holdTimer.Interval = HoldInterval;
    }

    private static void OnHoldMouseMove( object sender, MouseEventArgs e )
    {
      if ( sender is not TabControl tabControl )
        return;

      if ( Mouse.MiddleButton != MouseButtonState.Pressed )
      {
        EndHold();
        return;
      }

      var position = e.GetPosition( tabControl );
      if ( ( position - _holdStart ).Length < DragDistance )
        return;

      CloseUnderCursor( tabControl );
    }

    private static void OnHoldMouseUp( object sender, MouseButtonEventArgs e )
    {
      if ( e.ChangedButton != MouseButton.Middle )
        return;

      EndHold();
      e.Handled = true;
    }

    private static void OnHoldLostCapture( object sender, MouseEventArgs e )
    {
      if ( _endingHold || Mouse.MiddleButton != MouseButtonState.Pressed )
        EndHold();
    }

    private static void EndHold()
    {
      if ( _endingHold )
        return;

      _endingHold = true;

      _holdTimer?.Stop();
      if ( _holdTimer is not null )
        _holdTimer.Tick -= OnHoldTick;
      _holdTimer = null;

      if ( _holdTabControl is not null )
      {
        _holdTabControl.PreviewMouseMove -= OnHoldMouseMove;
        _holdTabControl.PreviewMouseUp -= OnHoldMouseUp;
        _holdTabControl.LostMouseCapture -= OnHoldLostCapture;
        if ( Mouse.Captured == _holdTabControl )
          _holdTabControl.ReleaseMouseCapture();
      }

      _holdTabControl = null;
      _lastClosed = null;
      _endingHold = false;
    }

    private static void CloseUnderCursor( TabControl tabControl )
    {
      var hit = tabControl.InputHitTest( Mouse.GetPosition( tabControl ) ) as DependencyObject;
      var tabItem = FindAncestor<TabItem>( hit );
      if ( tabItem is null || ReferenceEquals( tabItem, _lastClosed ) || !GetIsEnabled( tabItem ) )
        return;

      if ( Close( tabItem ) )
        _lastClosed = tabItem;
    }

    private static bool Close( TabItem tabItem )
    {
      var command = FindCloseCommand( tabItem );
      if ( command is null || !command.CanExecute( null ) )
        return false;

      command.Execute( null );
      return true;
    }

    private static ICommand FindCloseCommand( TabItem tabItem )
    {
      if ( tabItem.Header is ITabViewModel header )
        return header.CloseCommand;

      if ( tabItem.DataContext is ITabViewModel viewModel )
        return viewModel.CloseCommand;

      if ( tabItem.DataContext is FrameworkElement element && element.DataContext is ITabViewModel elementViewModel )
        return elementViewModel.CloseCommand;

      return null;
    }

    private static T FindAncestor<T>( DependencyObject source )
      where T : DependencyObject
    {
      while ( source is not null )
      {
        if ( source is T match )
          return match;

        source = source is Visual || source is System.Windows.Media.Media3D.Visual3D
          ? VisualTreeHelper.GetParent( source )
          : LogicalTreeHelper.GetParent( source );
      }

      return null;
    }

  }

}
