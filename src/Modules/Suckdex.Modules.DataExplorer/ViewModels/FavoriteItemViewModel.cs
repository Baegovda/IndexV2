using System.Windows.Input;
using Prism.Mvvm;

namespace Suckdex.Modules.DataExplorer.ViewModels
{

  public class FavoriteItemViewModel : BindableBase
  {

    public FavoriteItemViewModel( AssetNodeViewModel node, ICommand openCommand, ICommand removeCommand )
    {
      Node = node;
      Name = node.Name;
      OpenCommand = openCommand;
      RemoveCommand = removeCommand;
    }

    public AssetNodeViewModel Node { get; }

    public string Name { get; }

    public ICommand OpenCommand { get; }

    public ICommand RemoveCommand { get; }

  }

}
