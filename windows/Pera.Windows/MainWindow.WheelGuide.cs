using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Pera.Core;
namespace Pera.Windows;

/// A plain-language guide to the three drag wheels: what opens each one and what every choice does.
/// Format and tool names on a wheel are terse ("HEIC", "Add BG"); this window explains them.
public sealed partial class MainWindow
{
    private Window? wheelGuide;
    internal static readonly (string Id,string Description)[] ToolGuide=
    [
        ("compress","Makes photos, videos, audio and PDFs smaller."),("removeMetadata","Shows or removes hidden details such as location, camera and date."),
        ("editImage","Adjusts brightness, contrast, color and size."),("annotateImage","Adds arrows, shapes and text to a photo."),
        ("frameImage","Places a photo on a colored or blurred background."),("cropImage","Cuts away the edges of a photo."),
        ("createCollage","Arranges several photos into one image."),("redactImage","Blurs or blacks out private parts of a photo."),
        ("readQRCode","Finds and reads QR codes in photos and PDFs."),("trimVideo","Keeps only the part of a video you choose."),
        ("cropVideo","Cuts away the edges of a video frame."),("splitVideo","Cuts a video into several parts."),
        ("joinVideos","Joins several videos end to end."),("changeVideoSpeed","Speeds a video up or slows it down."),
        ("muteVideo","Removes the sound from a video."),("videoSnapshots","Saves still images from a video."),
        ("redactVideo","Blurs private areas in a video."),("trimAudio","Keeps only the part of a recording you choose."),
        ("normalizeAudio","Evens out quiet and loud parts of a recording."),("audioChannels","Switches audio between mono and stereo."),
        ("audioToVideo","Turns audio into a video with a moving waveform."),("redactAudio","Covers part of a recording with a beep."),
        ("createPDF","Turns photos or documents into one PDF."),("mergePDF","Combines several PDFs into one."),
        ("splitPDF","Separates a PDF into pages or ranges."),("organizePDF","Reorders, rotates or deletes PDF pages."),
        ("extractArchive","Opens ZIP, RAR, TAR and GZ archives.")
    ];
    internal static readonly (string Group,(string Format,string Description)[] Formats)[] FormatGuide=
    [
        ("Photos",[("JPG","The most common photo format. Small files that open everywhere."),("PNG","No quality loss and keeps transparency. Good for screenshots and logos."),("WEBP","Modern web format. Smaller than JPG at similar quality."),("HEIC","The iPhone photo format. Very small, but not every PC opens it."),("AVIF","The newest web format. Smallest files; support is still growing."),("TIFF","No quality loss, for printing and archiving. Large files."),("BMP","An old uncompressed Windows format. Very large files.")]),
        ("Video",[("MP4","Plays almost everywhere. The safest choice."),("MOV","Apple’s video format, handy for Mac and iPhone editing."),("MKV","A flexible format that can hold several audio and subtitle tracks."),("WEBM","An open video format made for web browsers."),("AVI","An older Windows video format."),("WMV","Windows Media video for older Windows players."),("GIF","A short, silent, looping animation.")]),
        ("Audio",[("MP3","The most common audio format. Plays everywhere."),("M4A","AAC audio: good quality at a small size."),("WAV","Uncompressed, full quality. Large files."),("FLAC","Full quality like WAV, but smaller."),("OGG","An open format often used in games and on Linux."),("OPUS","Very efficient for voice and streaming; some players cannot open it."),("AIFF","Apple’s uncompressed audio, like WAV."),("WMA","Windows Media audio for older players.")]),
        ("Documents",[("PDF","Looks the same everywhere. Ready to share and print."),("DOCX","A Microsoft Word document you can edit."),("PAGES","An Apple Pages document."),("EPUB","An e-book for reader apps and phones."),("HTML","A web page that opens in any browser."),("MD","Markdown: plain text with simple formatting."),("TXT","Plain text only, without formatting.")]),
        ("Spreadsheets and slides",[("XLSX","A Microsoft Excel workbook."),("ODS","An open spreadsheet for LibreOffice."),("CSV","A table as plain text, separated by commas."),("TSV","A table as plain text, separated by tabs."),("PPTX","A PowerPoint presentation.")]),
        ("Subtitles",[("SRT","The most common subtitle format."),("VTT","Subtitles for web video players.")]),
        ("Archives",[("ZIP","Packs many files into one smaller file. Opens everywhere."),("TAR","Bundles files together without compressing them."),("GZ","A compressed file common on Linux.")])
    ];
    internal static readonly (string Name,string Description)[] ActionGuide=
    [
        ("Share","Opens the Windows share panel."),("Float","Keeps files in a small floating window so you can drop them somewhere later."),
        ("Copy","Copies the files so you can paste them anywhere."),("Move","Moves the files to a folder you choose."),
        ("Copy Path","Copies the file’s full location as text."),("Terminal","Opens a command window in the file’s folder.")
    ];
    private void ShowWheelGuide()
    {
        if(wheelGuide?.IsVisible==true){wheelGuide.Activate();return;}
        var page=new StackPanel{Margin=new(28,24,28,28)};
        page.Children.Add(Text("How the wheels work",27));
        var intro=Text("Drag a file anywhere on your PC and hold a key: a wheel opens around the pointer. Move onto a choice and let go to apply it. Inside Pera, select files and press Ctrl+Space.",13,"Muted");intro.Margin=new(0,8,0,6);page.Children.Add(intro);
        var hint=Text("Each wheel shows only the choices that fit the files you drag. Your originals are never changed; Pera saves a new copy.",12,"Muted");hint.Margin=new(0,0,0,18);page.Children.Add(hint);
        var p=preferences.Value;
        page.Children.Add(GuideSection(p.ToolsModifier,"Tools wheel","Edit, trim, crop or combine your files.",GuideRows(ToolGuide.Where(t=>FeatureCatalog.ToolNames.ContainsKey(t.Id)).Select(t=>(FeatureCatalog.ToolNames[t.Id],t.Description,true)))));
        var formats=new StackPanel();foreach(var (group,items) in FormatGuide){var heading=Text(group,13,"Gold");heading.FontWeight=FontWeights.SemiBold;heading.Margin=new(0,formats.Children.Count==0?0:14,0,4);formats.Children.Add(heading);formats.Children.Add(GuideRows(items.Select(f=>(f.Format,f.Description,false))));}
        page.Children.Add(GuideSection(p.ConvertModifier,"Convert wheel","Save a copy of your file in another format. Not sure which one? These are the formats you will see:",formats));
        page.Children.Add(GuideSection(p.ActionsModifier,"File actions wheel","Quick things to do with the files themselves.",GuideRows(ActionGuide.Select(a=>(a.Name,a.Description,true)))));
        var note=Text("You can change these keys in Settings → General.",12,"Muted");note.Margin=new(4,6,0,0);page.Children.Add(note);
        var dialog=new Window{Owner=this,Title="Wheel guide · Pera",Width=720,Height=760,MinWidth=520,MinHeight=420,WindowStartupLocation=WindowStartupLocation.CenterOwner,Content=new ScrollViewer{Content=page,VerticalScrollBarVisibility=ScrollBarVisibility.Auto,HorizontalScrollBarVisibility=ScrollBarVisibility.Disabled}};
        Theme.Attach(dialog);
        dialog.PreviewKeyDown+=(_,e)=>{if(e.Key==Key.Escape){dialog.Close();e.Handled=true;}};dialog.Closed+=(_,_)=>wheelGuide=null;
        wheelGuide=dialog;dialog.Show();
    }
    private static Border GuideSection(string modifier,string title,string summary,UIElement body)
    {
        var stack=new StackPanel();var header=new StackPanel{Orientation=Orientation.Horizontal};
        var key=new Border{CornerRadius=new(8),Padding=new(10,4,10,4),Margin=new(0,0,12,0),VerticalAlignment=VerticalAlignment.Center,BorderThickness=new(1),Child=Text(modifier,13,localize:false)};key.SetResourceReference(Border.BorderBrushProperty,"Gold");
        header.Children.Add(key);header.Children.Add(Text(title,19));stack.Children.Add(header);
        var text=Text(summary,12,"Muted");text.Margin=new(0,8,0,14);stack.Children.Add(text);stack.Children.Add(body);
        var card=Card(stack);card.Margin=new(0,0,0,14);return card;
    }
    private static Grid GuideRows(IEnumerable<(string Name,string Description,bool LocalizeName)> rows)
    {
        var grid=new Grid();grid.ColumnDefinitions.Add(new(){Width=new(170)});grid.ColumnDefinitions.Add(new(){Width=new(1,GridUnitType.Star)});
        foreach(var (name,description,localizeName) in rows)
        {
            grid.RowDefinitions.Add(new(){Height=GridLength.Auto});var row=grid.RowDefinitions.Count-1;
            var label=Text(name,12,localize:localizeName);label.FontWeight=FontWeights.SemiBold;label.Margin=new(0,4,12,4);label.VerticalAlignment=VerticalAlignment.Top;Grid.SetRow(label,row);grid.Children.Add(label);
            var detail=Text(description,12,"Muted");detail.Margin=new(0,4,0,4);detail.VerticalAlignment=VerticalAlignment.Top;Grid.SetRow(detail,row);Grid.SetColumn(detail,1);grid.Children.Add(detail);
        }
        return grid;
    }
}
