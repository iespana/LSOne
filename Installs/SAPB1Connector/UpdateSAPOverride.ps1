$path = $env:ProgramData
$pluginToOverride = "LSOne.Connectors.SAPBusinessOne.SiteServicePlugin.dll"

if(-not (Test-Path ($path + '\LS Retail')))
{
    New-Item -Path $path -ItemType "directory" -Name 'LS Retail'
}

$path += "\LS Retail"

if(-not (Test-Path ($path + '\LS One Site Service')))
{
    New-Item -Path $path -ItemType "directory" -Name 'LS One Site Service'
}

$path += "\LS One Site Service"

if(-not (Test-Path ($path + '\LS One Site Service.config')))
{
    New-Item -Path $path -ItemType "file" -Name 'LS One Site Service.config'
}

$path += "\LS One Site Service.config"

[System.Collections.ArrayList]$fileContent = New-Object System.Collections.ArrayList<string>(,[System.IO.File]::ReadAllLines($path))

if($fileContent.Length -eq 0)
{
    $stream = [System.IO.StreamWriter] "$path"
    $stream.WriteLine("<configuration>")
    $stream.WriteLine("<appSettings>")
    $stream.WriteLine("<add key=`"LogLevel`" value=`"3`" />")
    $stream.WriteLine("<add key=`"SiteServicePluginOverride`" value =`"$pluginToOverride`" />")
    $stream.WriteLine("</appSettings>")
    $stream.WriteLine("</configuration>")
    $stream.Flush();
    $stream.Close();
}
else
{
    $key = ""
    $index = 0;
    Foreach($line in $fileContent)
    {
        if($line -match 'key="SiteServicePluginOverride"')
        {
            $key = $line;
            $index = [array]::IndexOf($fileContent, $line)
            break;
        }
    }

	if($key -eq "")
	{
		$key = "`t<add key=`"SiteServicePluginOverride`" value=`"$pluginToOverride`" />"
		$fileContent.Insert($fileContent.Count - 2, $key)
	}
	else
	{
		$fromIndex = $key.LastIndexOf("value=`"") + 7
		$toIndex = $key.LastIndexOf("`"")

		$key = $key.Remove($fromIndex, $toIndex - $fromIndex).Insert($fromIndex, $pluginToOverride)
		$fileContent[$index] = $key
	}
    
    $stream = [System.IO.StreamWriter] "$path"

    Foreach($line in $fileContent)
    {
        $stream.WriteLine($line)
    }

    $stream.Flush();
    $stream.Close();
}