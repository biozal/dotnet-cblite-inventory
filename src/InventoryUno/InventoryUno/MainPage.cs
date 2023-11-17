using InventoryUno.ViewModels;
using Microsoft.UI.Xaml.Media.Imaging;

namespace InventoryUno;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.DataContext<BindableMainModel>((page, vm) => page
            .Background(Theme.Brushes.Background.Default)
            .Content(
                new AutoLayout()
                    .Justify(AutoLayoutJustify.SpaceBetween)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Height(415)
                    .Children
                    (
                        new Image()
                            .Source(new BitmapImage(new Uri("https://picsum.photos/120/120")))
                            .Stretch(Stretch.UniformToFill)
                            .Width(120)
                            .Height(120)
                            .AutoLayout(counterAlignment: AutoLayoutAlignment.Center),
                        new AutoLayout()
                            .Background(Theme.Brushes.Background.Default)
                            .Width(375)
                            .AutoLayout(counterAlignment: AutoLayoutAlignment.Center)
                            .Children
                            (
                                new AutoLayout()
                                    .Spacing(20)
                                    .Padding(16)
                                    .Children
                                    (
                                        new TextBox()
                                            .Text(b => b.Bind(() => vm.Username).TwoWay())
                                            .PlaceholderText("Username")
                                            .Style(Theme.TextBox.Styles.Outlined),
                                        new PasswordBox()
                                            .PlaceholderText("Password")
                                            .Password(b => b.Bind(() => vm.Password).TwoWay()),
                                        new Button()
                                            .Content("Sign In")
                                            .Command(() => vm.Authenticate)
                                            .Style(Theme.Button.Styles.Elevated)
                                    )
                            )
                    )
            )
        );
    }
}
