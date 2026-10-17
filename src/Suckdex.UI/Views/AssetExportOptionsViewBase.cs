using System.Windows;
using System.Windows.Controls;

namespace Suckdex.UI.Views
{

  public class AssetExportOptionsViewBase : ContentControl
  {

    static AssetExportOptionsViewBase()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( AssetExportOptionsViewBase ),
        new FrameworkPropertyMetadata( typeof( AssetExportOptionsViewBase ) ) );
    }

    //public AssetExportOptionsViewBase()
    //{
    //  SetResourceReference( TemplateProperty, "AssetExportOptionsViewTemplate" );
    //}

  }

}
