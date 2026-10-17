using System.ComponentModel.DataAnnotations;

namespace Suckdex.Validation
{

  public class ValidatePathExistsAttribute : ValidationAttribute
  {

    public override bool IsValid( object? value )
    {
      if ( !( value is string pathValue ) )
        return false;

      if ( string.IsNullOrEmpty( pathValue ) )
        return false;

      return File.Exists( pathValue ) || Directory.Exists( pathValue );
    }

  }

}
