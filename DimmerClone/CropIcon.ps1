
Add-Type -AssemblyName System.Drawing

$iconPath = "$PSScriptRoot\Icon.png"
if (-not (Test-Path $iconPath)) {
    Write-Host "Icon.png not found at $iconPath"
    exit
}

$bmp = [System.Drawing.Bitmap]::FromFile($iconPath)
$width = $bmp.Width
$height = $bmp.Height

$minX = $width
$minY = $height
$maxX = 0
$maxY = 0

$hasPixels = $false

# Scan for non-transparent pixels
for ($x = 0; $x -lt $width; $x++) {
    for ($y = 0; $y -lt $height; $y++) {
        $pixel = $bmp.GetPixel($x, $y)
        if ($pixel.A -gt 0) {
            if ($x -lt $minX) { $minX = $x }
            if ($x -gt $maxX) { $maxX = $x }
            if ($y -lt $minY) { $minY = $y }
            if ($y -gt $maxY) { $maxY = $y }
            $hasPixels = $true
        }
    }
}

if (-not $hasPixels) {
    Write-Host "Image is fully transparent. No cropping needed."
    $bmp.Dispose()
    exit
}

# Add a small padding if desired, or crop tight.
# Let's crop tight as requested to make it "bigger".

$rect = New-Object System.Drawing.Rectangle($minX, $minY, ($maxX - $minX + 1), ($maxY - $minY + 1))
$croppedBmp = $bmp.Clone($rect, $bmp.PixelFormat)

$bmp.Dispose()

# Overwrite original
$croppedBmp.Save($iconPath, [System.Drawing.Imaging.ImageFormat]::Png)
$croppedBmp.Dispose()

Write-Host "Icon cropped successfully. New bounds: $rect"
