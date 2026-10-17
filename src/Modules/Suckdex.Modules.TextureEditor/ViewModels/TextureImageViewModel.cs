using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Suckdex.UI.ViewModels;

namespace Suckdex.Modules.TextureEditor.ViewModels
{

  public class TextureImageViewModel : ViewModelBase
  {

    public int ImageIndex { get; set; }
    public BitmapImage Preview { get; set; }

  }

}
