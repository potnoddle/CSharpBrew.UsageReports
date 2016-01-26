set-alias ?: Invoke-Ternary -Option AllScope -Description "PSCX filter alias"
filter Invoke-Ternary ([bool]$decider, [string]$ifTrue, [string]$ifFalse) 
{
   if ($decider) { 
      $ifTrue
   } else { 
      $ifFalse 
   }
}

function CleanUp($name)
{
    $name -csplit '(?<!^)(?=[A-Z][a-z])' -join ' '
}

function coalesce {
   Param ([string[]]$list)
   #$default = $list[-1] 
   $coalesced = ($list -ne $null)
   $coalesced[0]
}

function Extract-ZIPFile($file, $destination)
{
    $shell = new-object -com shell.application;
    $zip = $shell.NameSpace($file);

    foreach($item in $zip.items())
    {
        $shell.NameSpace($destination).Copyhere($item);
    }
}

function Bullets {
   Param (
       $selection,
       [string[]]$list,
       [string] $title
   )

    if ($list.Count -gt 0)
    {
        CreateHeading $selection 5 $title;

        $x = 1;     

        foreach($item in $list)
        {            
            $selection.TypeParagraph();
            $range = $selection.Range;
            $selection.TypeText($item);
            if ($x -eq 1)
            {
                $range.ListFormat.ApplyBulletDefault("Default");
            }
            $x++;
        }        
    }
}

function CreateHeading {
   Param (
       $selection,
       [int] $level,
       [string] $title,
       [bool] $newLine = $true
   )
    if ($newLine) {
        $selection.TypeParagraph();
    }
    $selection.style = "Heading $level";
    $selection.TypeText($title);

}

function CreateContentTypeSection($doc, $selection, $contents, $type) {
    CreateHeading $selection 3 "$type Types";

    $selection.TypeParagraph();
    $selection.style = "Normal";
    $selection.TypeText("A breakdown of all " + $type.toLower() + "s");

    foreach ($content in $contents)
    {
        CreateHeading $selection 4 (CleanUp $content.ContentTypeName);

        $selection.TypeParagraph();   
        $selection.style = "Normal";
        $selection.TypeText( (coalesce $content.Description, "No description"));

        if ($content.VisibleInMenu)
        {
            $selection.TypeText(", is visible in menu");
        }
        else
        {
            $selection.TypeText(", is not visible in menu");
        }

        if ($content.Available)
        {
            $selection.TypeText(" is available");
        }
        else
        {
            $selection.TypeText(" is unavailable");
        }

        if ($content.GroupName -ne "" -and $content.GroupName -ne $null)
        {
            $selection.TypeText(" and is a ");
            $selection.Font.Bold = 1;
            $selection.TypeText($content.GroupName + " " + $type);
            $selection.Font.Bold = 0;
        }

        $selection.TypeText(".");

        Bullets $selection $content.SupportedTemplates "Supported Templates";
        Bullets $selection $content.ExcludeContentTypeNames "Exclude Content";
        Bullets $selection $content.ExcludeOnContentTypeNames "Exclude On Content";
        Bullets $selection $content.IncludedContentTypeNames "Included Content";
        Bullets $selection $content.IncludedOnContentTypeNames "Included On Content";
        
        if ($content.Properties.count -gt 0)
        {         
            CreateHeading $selection 5 "Properties";
            $totalRows = $content.Properties.count;

            $table = $doc.Tables.Add($selection.Range, $totalRows + 1, 7);
            $table.Columns.AutoFit();

            $p = 1;    
            $table.Cell($p,1).Range.Text = "Name";
            $table.Cell($p,2).Range.Text = "EPiServer Type";
            $table.Cell($p,3).Range.Text = "Help Text";
            $table.Cell($p,4).Range.Text = "Tab Name";
            $table.Cell($p,5).Range.Font.Size = 8;
            $table.Cell($p,5).Range.Orientation = 3;
            $table.Cell($p,5).Range.Text = "Lang Unique";
            $table.Cell($p,6).Range.Font.Size = 8;
            $table.Cell($p,6).Range.Orientation = 3;
            $table.Cell($p,6).Range.Text = "Searchable";
            $table.Cell($p,7).Range.Font.Size = 8;
            $table.Cell($p,7).Range.Orientation = 3;
            $table.Cell($p,7).Range.Text = "Required";

            $p++;
            foreach ($property in $content.Properties | Sort-Object TabName)
            { 
                $table.Cell($p,1).Range.Text = Coalesce $property.EditCaption, (CleanUp $property.PropertyName);
                $table.Cell($p,2).Range.Text = CleanUp ($property.TypeName -replace "Property", "");
                $table.Cell($p,3).Range.Text = $property.HelpText;
                $table.Cell($p,4).Range.Text = $property.TabName;
                $table.Cell($p,5).Range.Text = ?: $property.LanguageSpecific "T" "F";
                $table.Cell($p,6).Range.Text = ?: $property.Searchable "T" "F";
                $table.Cell($p,7).Range.Text = ?: $property.Required "T" "F";
                $p++;
            }
        
            $table.AutoFormat(1) | Out-Null;
            $table.Style = "Grid Table 4 - Accent 1";

            $selection.EndKey(6) | Out-Null;
            #break;
        }
    }
}

function ExtractJson($root, $tmpFolder) {
    if(Test-Path -Path "$root\$tmpFolder" ) {
        Remove-Item -Path "$root\$tmpFolder\*" -Force
    } else {
        New-Item -ItemType directory -Path "$root\$tmpFolder"
    }
    Extract-ZIPFile –File "$root\UsageReport.zip" –Destination "$root\$tmpFolder"
}

$wdTypes = Add-Type -AssemblyName 'Microsoft.Office.Interop.Word' -Passthru; 
$wdPageBreak = [Enum]::Parse([Microsoft.Office.Interop.Word.WdBreakType], "wdPageBreak");
$wdDoc = [Enum]::Parse([Microsoft.Office.Interop.Word.WdSaveFormat], "wdFormatDocument");
$wdPDF = [Enum]::Parse([Microsoft.Office.Interop.Word.WdSaveFormat], "wdFormatPDF");

$root = $PSScriptRoot;
$tmpFolder = "report-temp";

ExtractJson $root $tmpFolder

$contentTypes = Get-Content -Raw -Path "$root\$tmpFolder\contentproperties-report.json" | ConvertFrom-Json;
$now = Get-Date;

$word = New-Object -ComObject word.application;

$word.visible = $true;
$doc = $word.documents.add("$root\DefaultTemplate.dotx");

$doc.BuiltInDocumentProperties["Title"].Value = "EPiServer Content Type Report";
$doc.BuiltInDocumentProperties["Subject"].Value = "An Automaticly generated epierver Report on the content types";

$selection = $word.selection;
$selection.EndKey(6) | Out-Null;
$selection.InsertBreak($wdPageBreak);

CreateHeading $selection 1 "Introduction" $false;

$selection.TypeParagraph();
$selection.style = "Normal";
$selection.TypeText("This document is divided into multiple sections and was automatically generated on $now.");

CreateHeading $selection 2 "Content Types";

$selection.TypeParagraph();
$selection.style = "Normal";
$selection.TypeText("A list of all content types broken down by type.");


$pages = $contentTypes | Where-Object {$_.IsPage -eq $true}
CreateContentTypeSection $doc $selection $pages "Page"

$blocks = $contentTypes | Where-Object {$_.IsBlock -eq $true}
CreateContentTypeSection $doc $selection $blocks "Block"

$files = $contentTypes | Where-Object {$_.IsMedia -eq $true}
CreateContentTypeSection $doc $selection $files "Media";

$doc.TablesOfContents(1).Update();

$doc.saveas([ref] "$root\Content-Type-Report", [ref]$wdDoc);
$doc.saveas([ref] "$root\Content-Type-Report", [ref]$wdPDF);

$doc.Close();
$word.quit();