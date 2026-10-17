using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Suckdex.UI.Controls
{

  public class IxTextBox : TextBox
  {

    #region Data Members

    private bool _isSubmitting;
    private FrameworkElement? _historyPanel;
    private ListBox? _historyList;
    private INotifyCollectionChanged? _historyChanges;

    #endregion

    #region Properties

    public static readonly DependencyProperty HasTextProperty = DependencyProperty.Register(
      nameof( HasText ),
      typeof( bool ),
      typeof( IxTextBox ) );

    public bool HasText
    {
      get => ( bool ) GetValue( HasTextProperty );
      set => SetValue( HasTextProperty, value );
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
      nameof( Icon ),
      typeof( object ),
      typeof( IxTextBox ) );

    public object Icon
    {
      get => ( object ) GetValue( IconProperty );
      set => SetValue( IconProperty, value );
    }

    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
      nameof( PlaceholderText ),
      typeof( string ),
      typeof( IxTextBox ) );

    public string PlaceholderText
    {
      get => ( string ) GetValue( PlaceholderTextProperty );
      set => SetValue( PlaceholderTextProperty, value );
    }

    public static readonly DependencyProperty SubmitTextCommandProperty = DependencyProperty.Register(
      nameof( SubmitTextCommand ),
      typeof( ICommand ),
      typeof( IxTextBox ) );

    public ICommand SubmitTextCommand
    {
      get => ( ICommand ) GetValue( SubmitTextCommandProperty );
      set => SetValue( SubmitTextCommandProperty, value );
    }

    public static readonly DependencyProperty SearchHistoryProperty = DependencyProperty.Register(
      nameof( SearchHistory ),
      typeof( IEnumerable ),
      typeof( IxTextBox ),
      new PropertyMetadata( null, OnSearchHistoryChanged ) );

    public IEnumerable? SearchHistory
    {
      get => ( IEnumerable? ) GetValue( SearchHistoryProperty );
      set => SetValue( SearchHistoryProperty, value );
    }

    public static readonly DependencyProperty RemoveHistoryItemCommandProperty = DependencyProperty.Register(
      nameof( RemoveHistoryItemCommand ),
      typeof( ICommand ),
      typeof( IxTextBox ) );

    public ICommand? RemoveHistoryItemCommand
    {
      get => ( ICommand? ) GetValue( RemoveHistoryItemCommandProperty );
      set => SetValue( RemoveHistoryItemCommandProperty, value );
    }

    #endregion

    #region Constructor

    static IxTextBox()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxTextBox ),
        new FrameworkPropertyMetadata( typeof( IxTextBox ) ) );
    }

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      if ( _historyList is not null )
        _historyList.PreviewMouseLeftButtonDown -= OnHistoryItemPressed;

      _historyPanel = GetTemplateChild( "PART_HistoryPanel" ) as FrameworkElement;
      _historyList = GetTemplateChild( "PART_HistoryList" ) as ListBox;

      if ( _historyList is not null )
        _historyList.PreviewMouseLeftButtonDown += OnHistoryItemPressed;

      RefreshHistory();
    }

    protected override void OnTextChanged( TextChangedEventArgs e )
    {
      base.OnTextChanged( e );
      HasText = Text.Length != 0;
    }

    protected override void OnPreviewKeyDown( KeyEventArgs e )
    {
      base.OnPreviewKeyDown( e );

      if ( _historyList is not null && _historyList.Items.Count > 0 )
      {
        if ( e.Key == Key.Down )
        {
          _historyList.SelectedIndex = Math.Min( _historyList.Items.Count - 1, _historyList.SelectedIndex + 1 );
          e.Handled = true;
          return;
        }

        if ( e.Key == Key.Up )
        {
          _historyList.SelectedIndex = Math.Max( -1, _historyList.SelectedIndex - 1 );
          e.Handled = true;
          return;
        }
      }

      if ( e.Key == Key.Enter )
      {
        if ( _historyList?.SelectedItem is string selected )
          Text = selected;

        SubmitText();
      }
    }

    #endregion

    #region Private Methods

    private static void OnSearchHistoryChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
    {
      var textBox = ( IxTextBox ) sender;
      if ( textBox._historyChanges is not null )
        textBox._historyChanges.CollectionChanged -= textBox.OnHistoryCollectionChanged;

      textBox._historyChanges = e.NewValue as INotifyCollectionChanged;
      if ( textBox._historyChanges is not null )
        textBox._historyChanges.CollectionChanged += textBox.OnHistoryCollectionChanged;

      textBox.RefreshHistory();
    }

    private void OnHistoryCollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
      => RefreshHistory();

    private void RefreshHistory()
    {
      if ( _historyList is null )
        return;

      var items = ( SearchHistory ?? Array.Empty<string>() )
        .OfType<string>()
        .Where( term => !string.IsNullOrWhiteSpace( term ) )
        .ToList();

      _historyList.ItemsSource = items;
      if ( _historyPanel is not null )
        _historyPanel.Visibility = items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnHistoryItemPressed( object sender, MouseButtonEventArgs e )
    {
      if ( ( e.OriginalSource as FrameworkElement )?.DataContext is not string term )
        return;

      if ( IsRemoveRequest( e.OriginalSource as DependencyObject ) )
      {
        var removeCommand = RemoveHistoryItemCommand;
        if ( removeCommand?.CanExecute( term ) == true )
          removeCommand.Execute( term );

        e.Handled = true;
        Keyboard.Focus( this );
        RefreshHistory();
        return;
      }

      Text = term;
      CaretIndex = Text.Length;
      SubmitText();
      e.Handled = true;
    }

    private static bool IsRemoveRequest( DependencyObject? source )
    {
      while ( source is not null )
      {
        if ( source is FrameworkElement element && Equals( element.Tag, "RemoveHistory" ) )
          return true;

        source = VisualTreeHelper.GetParent( source );
      }

      return false;
    }

    private void SubmitText()
    {
      var submitCommand = SubmitTextCommand;
      if ( _isSubmitting || submitCommand is null )
        return;

      IsEnabled = false;
      _isSubmitting = true;
      var text = Text;

      Task.Run( () =>
      {
        submitCommand.Execute( text );
      } )
        .ContinueWith( t =>
        {
          _isSubmitting = false;
          Dispatcher.Invoke( () => { IsEnabled = true; } );
        } );
    }

    #endregion

  }

}
