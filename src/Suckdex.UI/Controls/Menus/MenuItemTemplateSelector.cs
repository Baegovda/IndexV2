using System.Windows;
using System.Windows.Controls;

namespace Suckdex.UI.Controls.Menus
{

  public class MenuItemTemplateSelector : ItemContainerTemplateSelector
  {

    public override DataTemplate SelectTemplate( object item, ItemsControl parentItemsControl )
    {
      var key = new DataTemplateKey( item.GetType() );
      return ( DataTemplate ) parentItemsControl.FindResource( key );
    }

  }

}
