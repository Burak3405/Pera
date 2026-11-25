using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Pera.Core;
namespace Pera.Windows;
public sealed partial class SettingsWindow
{
    internal static readonly IReadOnlyDictionary<string,string> AccentNames=new Dictionary<string,string>{["sage"]="Sage",["gold"]="Gold",["amber"]="Amber",["coral"]="Coral",["red"]="Red",["pink"]="Pink",["purple"]="Purple",["blue"]="Blue",["teal"]="Teal",["graphite"]="Graphite"};
    // Settings → Customize wheel: everything about how the wheel looks, in one place.
    private void WheelCustomization()
    {
        Heading("Make the wheel yours.","Choose how the wheel looks. Changes apply the next time a wheel opens; try it with the button below.");
        Pick("Wheel finish",["glass","solid","pera"],store.Value.WheelFinish,v=>{Save(store.Value with{WheelFinish=v});RefreshWheelSection();});
        if(store.Value.WheelFinish=="glass")WheelSlider("Glass opacity",.4,1,store.Value.WheelOpacity,v=>store.Value with{WheelOpacity=v});
        page.Children.Add(L10n.Label(new TextBlock{Text="Selection color",Margin=new(4,16,4,6)}));
        var swatches=new WrapPanel{Margin=new(0,0,0,4)};
        foreach(var id in Preferences.WheelAccents)
        {
            var colors=WheelPresentation.Accents[id];var chosen=id==store.Value.WheelAccent;
            var dot=new Ellipse{Width=26,Height=26,Fill=new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors.Stroke)),Stroke=chosen?Theme.Brush("Ink"):Brushes.Transparent,StrokeThickness=3};
            var swatch=new Button{Content=dot,Width=40,Height=40,Padding=new(0),Margin=new(2),ToolTip=L10n.Text(AccentNames[id])};
            System.Windows.Automation.AutomationProperties.SetName(swatch,L10n.Text(AccentNames[id]));System.Windows.Automation.AutomationProperties.SetAutomationId(swatch,"WheelAccent-"+id);
            swatch.Click+=(_,_)=>{Save(store.Value with{WheelAccent=id});RefreshWheelSection();};swatches.Children.Add(swatch);
        }
        page.Children.Add(swatches);
        WheelSlider("Gap between slices",0,6,store.Value.WheelGap,v=>store.Value with{WheelGap=v});
        Pick("Slice labels",["iconAndText","iconOnly","textOnly"],store.Value.WheelLabels,v=>Save(store.Value with{WheelLabels=v}));
        Pick("Wheel center",["wheelName","peraLogo","blank"],store.Value.WheelCenter,v=>Save(store.Value with{WheelCenter=v}));
        Pick("Opening animation",["fadeIn","grow","spin"],store.Value.WheelAnimation,v=>Save(store.Value with{WheelAnimation=v}));
        if(!store.Value.Animations)page.Children.Add(L10n.Label(new TextBlock{Text="Turn on Subtle animations in General to see the opening animation.",TextWrapping=TextWrapping.Wrap,Foreground=Theme.Brush("Muted"),Margin=new(4,6,4,0)}));
        WheelSlider("Wheel size",.85,1.4,store.Value.WheelScale,v=>store.Value with{WheelScale=v});
        var actions=new WrapPanel{Margin=new(0,20,0,0)};
        actions.Children.Add(MainWindow.Button("Try the wheel",TryWheel));
        actions.Children.Add(MainWindow.Button("Reset wheel look",()=>{var d=new Preferences();Save(store.Value with{WheelFinish=d.WheelFinish,WheelOpacity=d.WheelOpacity,WheelAccent=d.WheelAccent,WheelGap=d.WheelGap,WheelLabels=d.WheelLabels,WheelCenter=d.WheelCenter,WheelAnimation=d.WheelAnimation,WheelScale=d.WheelScale});RefreshWheelSection();}));
        foreach(var child in actions.Children.OfType<FrameworkElement>())child.Margin=new(0,0,10,0);
        page.Children.Add(actions);
    }
    private void WheelSlider(string title,double minimum,double maximum,double value,Func<double,Preferences> change)
    {
        page.Children.Add(L10n.Label(new TextBlock{Text=title,Margin=new(4,16,4,0)}));
        var slider=new Slider{Minimum=minimum,Maximum=maximum,Value=value,Margin=new(4,8,4,8),ToolTip=L10n.Text(title)};System.Windows.Automation.AutomationProperties.SetName(slider,L10n.Text(title));
        // Save once the thumb settles instead of on every pixel of a drag.
        slider.ValueChanged+=(_,_)=>{if(!slider.IsMouseCaptureWithin)Save(change(Math.Round(slider.Value,2)));};
        slider.LostMouseCapture+=(_,_)=>Save(change(Math.Round(slider.Value,2)));
        page.Children.Add(slider);
    }
    private void RefreshWheelSection(){var offset=(page.Parent as ScrollViewer)?.VerticalOffset??0;page.Children.Clear();WheelCustomization();if(page.Parent is ScrollViewer viewer){viewer.UpdateLayout();viewer.ScrollToVerticalOffset(offset);}}
    private void TryWheel()
    {
        WheelPresentation.Preferences=store.Value;
        var sample=FeatureCatalog.ToolNames.Take(6).Select(pair=>new ToolChoice(pair.Key,pair.Value,MainWindow.ToolSymbol(pair.Key))).ToArray();
        new WheelWindow(this,sample,_=>{}).Show();
    }
}
