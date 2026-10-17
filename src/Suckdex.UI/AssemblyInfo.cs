using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
)]

[assembly: XmlnsDefinition( "http://index/ui", nameof( Suckdex.UI ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Suckdex.UI.Controls ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Suckdex.UI.Converters ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Suckdex.UI.Extensions ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Suckdex.UI.Services ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Suckdex.UI.Windows ) )]
