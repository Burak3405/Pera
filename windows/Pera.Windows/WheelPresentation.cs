using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Pera.Core;
namespace Pera.Windows;
internal static class WheelPresentation
{
    internal const double Diameter=320;
    internal static double StartAngle(int count)=>count==3?-180:-90;
    internal static Preferences Preferences{get;set;}=new();
    // Selection colours: (light theme fill, dark theme fill, outline). "sage" keeps the original theme colours.
    internal static readonly IReadOnlyDictionary<string,(string Light,string Dark,string Stroke)> Accents=new Dictionary<string,(string,string,string)>
    {
        ["sage"]=("#E0EBD2","#243928","#E3B859"),["gold"]=("#F6DFA6","#4F3D14","#C9962E"),["amber"]=("#FAD6AE","#57361A","#E08A2E"),
        ["coral"]=("#F8D9CC","#5A2A18","#D85A30"),["red"]=("#F6CFCF","#5C1E1E","#E24B4A"),["pink"]=("#F8D6E3","#5A2236","#D4537E"),
        ["purple"]=("#E0DDFA","#322C63","#7F77DD"),["blue"]=("#CFE3F7","#1E3A5A","#378ADD"),["teal"]=("#C9EDE2","#10403A","#1D9E75"),["graphite"]=("#DADBD6","#3A3D3A","#6B6E69")
    };
    // Slice colours of the "Pera colors" finish, taken from the logo.
    private static readonly string[] PeraSlices=["#E8B04F","#D99A3E","#EDC06D","#D3913A","#E8B04F","#F0CB86"];
    private static SolidColorBrush Solid(string hex,double opacity=1){var brush=new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex)){Opacity=opacity};brush.Freeze();return brush;}
    private static bool Pera=>Preferences.WheelFinish=="pera";
    private static double SliceOpacity=>Preferences.WheelFinish=="glass"?Preferences.WheelOpacity:1;
    internal static Brush Backdrop{get{if(Pera)return Solid("#FFFEF7");var brush=Theme.Brush("Surface").Clone();brush.Opacity=Preferences.WheelFinish=="glass"?Preferences.WheelOpacity*.6:1;return brush;}}
    /// The Pera finish keeps light slices in both themes, so its text stays dark.
    internal static Brush Ink=>Pera?Solid("#243B2B"):Theme.Brush("Ink");
    internal static Brush SliceFill(int index,bool selected)
    {
        if(selected){var accent=Accents.GetValueOrDefault(Preferences.WheelAccent,Accents["sage"]);return Preferences.WheelAccent=="sage"&&!Pera?WithOpacity(Theme.Brush("Sage"),Math.Min(1,SliceOpacity+.2)):Solid(Theme.Dark&&!Pera?accent.Dark:accent.Light,Math.Min(1,SliceOpacity+.2));}
        return Pera?Solid(PeraSlices[index%PeraSlices.Length]):WithOpacity(Theme.Brush("Inset"),SliceOpacity);
    }
    internal static Brush SliceStroke(bool selected)=>selected?(Preferences.WheelAccent=="sage"?Theme.Brush("Gold"):Solid(Accents.GetValueOrDefault(Preferences.WheelAccent,Accents["sage"]).Stroke)):Pera?Solid("#FFFEF7"):WithOpacity(Theme.Brush("Line"),Math.Min(1,SliceOpacity+.15));
    private static Brush WithOpacity(Brush brush,double opacity){if(opacity>=1)return brush;var copy=brush.Clone();copy.Opacity=opacity;copy.Freeze();return copy;}
    /// Half the angle of one slice, in radians, after the gap chosen in Settings.
    internal static double HalfAngle(int count)=>(180d/count-Math.Clamp(Preferences.WheelGap,0,6))*Math.PI/180;
    /// What the wheel's centre shows for a tool wheel; informational messages are always shown as text.
    internal static UIElement? CenterArt(double size){if(Preferences.WheelCenter=="blank")return new Border();if(Preferences.WheelCenter!="peraLogo")return null;return new System.Windows.Controls.Image{Source=FileGarden.Artwork("PeraMark"),Width=size,Height=size};}
    internal static double LabelRadius(int count,double half)=>count<=2?180:4*Math.Sin(half)*(Math.Pow(250,3)-Math.Pow(108,3))/(6*half*(250*250-108*108));
    // WPF wraps a word wider than its box between letters ("ANNOTAT / E"), so a caption only fits when every word does.
    internal static bool WordsFit(string text,double font,FontWeight weight,double width)=>text.Split(' ',StringSplitOptions.RemoveEmptyEntries).All(word=>WordWidth(word,font,weight)<=width);
    private static double WordWidth(string word,double font,FontWeight weight){var text=new System.Windows.Controls.TextBlock{Text=word,FontSize=font,FontWeight=weight};text.Measure(new Size(double.PositiveInfinity,double.PositiveInfinity));return text.DesiredSize.Width;}
    internal static (StackPanel Label,Point Center) Caption(ToolChoice choice,Geometry petal,double angle,double half,int count)
    {
        var preferred=LabelRadius(count,half);var title=L10n.Uppercase(choice.Title);var ink=Ink;
        var showIcon=Preferences.WheelLabels!="textOnly";var showText=Preferences.WheelLabels!="iconOnly";
        for(var font=16d;font>=10;font-=.5)for(var width=142d;width>=72;width-=7)
        {
            if(showText&&!WordsFit(title,font,FontWeights.SemiBold,width))continue;
            var label=new StackPanel{Width=width,IsHitTestVisible=false};
            if(showIcon&&choice.Placement is {} placement){var glyph=new System.Windows.Controls.Canvas{Width=36,Height=25,HorizontalAlignment=HorizontalAlignment.Center,Margin=new(0,0,0,showText?7:0)};glyph.Children.Add(new System.Windows.Shapes.Rectangle{Width=35,Height=24,RadiusX=3,RadiusY=3,Stroke=ink,StrokeThickness=1.7});var (start,end)=placement.Range();var fill=new System.Windows.Shapes.Rectangle{Width=29*(end-start),Height=18,Fill=ink,Opacity=.65};System.Windows.Controls.Canvas.SetLeft(fill,3+29*start);System.Windows.Controls.Canvas.SetTop(fill,3);glyph.Children.Add(fill);label.Children.Add(glyph);}
            else if(showIcon)label.Children.Add(new System.Windows.Controls.TextBlock{Text=choice.Symbol,FontSize=showText?32:44,TextAlignment=TextAlignment.Center,Foreground=ink,Margin=new(0,0,0,showText?6:0)});
            if(showText)label.Children.Add(new System.Windows.Controls.TextBlock{Text=title,FontSize=font,FontWeight=FontWeights.SemiBold,TextAlignment=TextAlignment.Center,TextWrapping=TextWrapping.Wrap,Foreground=ink});
            // Icon-only slices keep the title for screen readers and tooltips.
            if(!showText)System.Windows.Automation.AutomationProperties.SetName(label,title);
            label.Measure(new Size(width,double.PositiveInfinity));var h=label.DesiredSize.Height;
            for(var step=0;step<=20;step++)
            {
                var radial=preferred+(step==0?0:(step%2==1?1:-1)*((step+1)/2)*5);var center=new Point(270+radial*Math.Cos(angle),270+radial*Math.Sin(angle));var r=new Rect(center.X-width/2-3,center.Y-h/2-3,width+6,h+6);
                if(new[]{r.TopLeft,r.TopRight,r.BottomLeft,r.BottomRight,new Point(r.Left,r.Top+r.Height/2),new Point(r.Right,r.Top+r.Height/2)}.All(petal.FillContains))return(label,center);
            }
        }
        var small=10d;while(small>7&&!WordsFit(title,small,FontWeights.Normal,72))small-=.5;
        var fallback=new StackPanel{Width=72};fallback.Children.Add(new System.Windows.Controls.TextBlock{Text=title,FontSize=small,TextAlignment=TextAlignment.Center,TextWrapping=TextWrapping.Wrap,Foreground=ink});fallback.Measure(new Size(72,double.PositiveInfinity));return(fallback,new Point(270+preferred*Math.Cos(angle),270+preferred*Math.Sin(angle)));
    }
    internal static void Animate(Window window,double from=0)
    {
        if(!Preferences.Animations||!SystemParameters.ClientAreaAnimation)return;
        var motion=Preferences.WheelAnimation;var duration=TimeSpan.FromMilliseconds(motion=="fadeIn"?140:240);
        window.BeginAnimation(UIElement.OpacityProperty,new DoubleAnimation(from,1,duration));
        if(motion=="fadeIn"||window.Content is not FrameworkElement content)return;
        var scale=new ScaleTransform(1,1);var transforms=new TransformGroup();transforms.Children.Add(scale);content.RenderTransformOrigin=new Point(.5,.5);
        var ease=new CubicEase{EasingMode=EasingMode.EaseOut};
        scale.BeginAnimation(ScaleTransform.ScaleXProperty,new DoubleAnimation(.72,1,duration){EasingFunction=ease});scale.BeginAnimation(ScaleTransform.ScaleYProperty,new DoubleAnimation(.72,1,duration){EasingFunction=ease});
        if(motion=="spin"){var rotate=new RotateTransform();transforms.Children.Add(rotate);rotate.BeginAnimation(RotateTransform.AngleProperty,new DoubleAnimation(-60,0,duration){EasingFunction=ease});}
        content.RenderTransform=transforms;
    }
}
