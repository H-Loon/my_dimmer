
$source = "$PSScriptRoot\Icon.png"
$dest = "$PSScriptRoot\Icon.ico"

if (-not (Test-Path $source)) {
    Write-Error "Source file not found: $source"
    exit 1
}

$pngBytes = [System.IO.File]::ReadAllBytes($source)
$pngSize = $pngBytes.Length
$width = 0   # 0 means 256
$height = 0  # 0 means 256
$bpp = 32

# Create ICO Header
# Reserved (2) + Type (2) + Count (2)
$header = [System.BitConverter]::GetBytes([uint16]0) + 
[System.BitConverter]::GetBytes([uint16]1) + 
[System.BitConverter]::GetBytes([uint16]1) 

# Create Icon Directory Entry
# Width(1) + Height(1) + Colors(1) + Res(1) + Planes(2) + BPP(2) + Size(4) + Offset(4)
$offset = 6 + 16 # Header(6) + 1 Entry(16)

$entry = [byte]$width, [byte]$height, 0, 0 +
[System.BitConverter]::GetBytes([uint16]1) + 
[System.BitConverter]::GetBytes([uint16]$bpp) + 
[System.BitConverter]::GetBytes([uint32]$pngSize) + 
[System.BitConverter]::GetBytes([uint32]$offset)

# Write file
$fs = [System.IO.File]::Create($dest)
$fs.Write($header, 0, $header.Length)
$fs.Write($entry, 0, $entry.Length)
$fs.Write($pngBytes, 0, $pngBytes.Length)
$fs.Close()

Write-Host "Created Icon.ico ($($pngSize) bytes wrapped) from Icon.png"
