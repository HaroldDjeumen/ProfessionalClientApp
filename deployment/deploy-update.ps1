# ProfessionalClientApp Deployment Script
# This script automates the process of building, signing, and deploying updates

param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    
    [Parameter(Mandatory=$false)]
    [string]$ReleaseNotes = "Bug fixes and improvements",
    
    [Parameter(Mandatory=$false)]
    [string]$ServerPath = "updates/",
    
    [Parameter(Mandatory=$false)]
    [string]$PrivateKeyPath = "NetSparkle_Ed25519.priv",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipUpload
)

# Configuration
$ProjectPath = "../ClienApp/ClientApp.csproj"
$OutputDir = "bin/Release/net8.0-windows"
$AppName = "ProfessionalClientApp"
$AppCastFile = "appcast.xml"

Write-Host "=== ProfessionalClientApp Deployment Script ===" -ForegroundColor Green
Write-Host "Version: $Version" -ForegroundColor Yellow
Write-Host "Release Notes: $ReleaseNotes" -ForegroundColor Yellow
Write-Host ""

# Step 1: Clean and build the application
Write-Host "Step 1: Building application..." -ForegroundColor Cyan
try {
    Set-Location "../ClienApp"
    
    # Clean previous builds
    dotnet clean --configuration Release
    
    # Build the application
    dotnet build --configuration Release --verbosity minimal
    
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed with exit code $LASTEXITCODE"
    }
    
    Write-Host "✓ Build completed successfully" -ForegroundColor Green
}
catch {
    Write-Host "✗ Build failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
finally {
    Set-Location "../deployment"
}

# Step 2: Create release directory structure
Write-Host "Step 2: Creating release directory..." -ForegroundColor Cyan
$ReleaseDir = "releases/$Version"
$SignatureDir = "signatures"

New-Item -ItemType Directory -Force -Path $ReleaseDir | Out-Null
New-Item -ItemType Directory -Force -Path $SignatureDir | Out-Null

# Step 3: Copy and rename executable
Write-Host "Step 3: Preparing executable..." -ForegroundColor Cyan
$SourceExe = "../ClienApp/$OutputDir/ClientApp.exe"
$TargetExe = "$ReleaseDir/$AppName-$Version.exe"

if (Test-Path $SourceExe) {
    Copy-Item $SourceExe $TargetExe
    Write-Host "✓ Executable copied to $TargetExe" -ForegroundColor Green
} else {
    Write-Host "✗ Source executable not found: $SourceExe" -ForegroundColor Red
    exit 1
}

# Step 4: Generate signature
Write-Host "Step 4: Generating signature..." -ForegroundColor Cyan
try {
    if (Test-Path $PrivateKeyPath) {
        $SignatureOutput = & netsparkle-generate-signature --private-key $PrivateKeyPath --file $TargetExe 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            # Extract signature from output
            $Signature = ($SignatureOutput | Select-String "Signature: (.+)").Matches[0].Groups[1].Value
            
            # Save signature to file
            $SignatureFile = "$SignatureDir/$AppName-$Version.exe.signature"
            $Signature | Out-File -FilePath $SignatureFile -Encoding ASCII
            
            Write-Host "✓ Signature generated: $Signature" -ForegroundColor Green
        } else {
            throw "Signature generation failed: $SignatureOutput"
        }
    } else {
        Write-Host "⚠ Private key not found at $PrivateKeyPath. Skipping signature generation." -ForegroundColor Yellow
        $Signature = "SIGNATURE_PLACEHOLDER"
    }
}
catch {
    Write-Host "✗ Signature generation failed: $($_.Exception.Message)" -ForegroundColor Red
    $Signature = "SIGNATURE_PLACEHOLDER"
}

# Step 5: Get file size
$FileSize = (Get-Item $TargetExe).Length
Write-Host "✓ File size: $FileSize bytes" -ForegroundColor Green

# Step 6: Update appcast.xml
Write-Host "Step 5: Updating appcast..." -ForegroundColor Cyan
try {
    $AppCastPath = $AppCastFile
    $CurrentDate = Get-Date -Format "ddd, dd MMM yyyy HH:mm:ss +0000"
    
    # Create new item XML
    $NewItem = @"
        <item>
            <title>Version $Version</title>
            <description><![CDATA[
                <h3>Version $Version</h3>
                <p>$ReleaseNotes</p>
            ]]></description>
            <pubDate>$CurrentDate</pubDate>
            <enclosure url="https://yourdomain.com/$ServerPath/releases/$Version/$AppName-$Version.exe"
                       sparkle:version="$Version"
                       sparkle:os="windows"
                       length="$FileSize"
                       type="application/octet-stream"
                       sparkle:signature="$Signature" />
        </item>
"@

    if (Test-Path $AppCastPath) {
        # Read existing appcast
        $AppCastContent = Get-Content $AppCastPath -Raw
        
        # Insert new item after the channel opening tag
        $UpdatedContent = $AppCastContent -replace '(<channel>.*?<language>.*?</language>)', "`$1`n$NewItem"
        
        # Write updated content
        $UpdatedContent | Out-File -FilePath $AppCastPath -Encoding UTF8
    } else {
        # Create new appcast file
        $NewAppCast = @"
<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0" xmlns:sparkle="http://www.andymatuschak.org/xml-namespaces/sparkle">
    <channel>
        <title>$AppName Updates</title>
        <link>https://yourdomain.com/$ServerPath/$AppCastFile</link>
        <description>Updates for $AppName</description>
        <language>en</language>
$NewItem
    </channel>
</rss>
"@
        $NewAppCast | Out-File -FilePath $AppCastPath -Encoding UTF8
    }
    
    Write-Host "✓ Appcast updated successfully" -ForegroundColor Green
}
catch {
    Write-Host "✗ Failed to update appcast: $($_.Exception.Message)" -ForegroundColor Red
}

# Step 6: Display deployment summary
Write-Host ""
Write-Host "=== Deployment Summary ===" -ForegroundColor Green
Write-Host "Version: $Version"
Write-Host "Executable: $TargetExe"
Write-Host "File Size: $FileSize bytes"
Write-Host "Signature: $($Signature.Substring(0, [Math]::Min(20, $Signature.Length)))..."
Write-Host "Appcast: $AppCastPath"
Write-Host ""

# Step 7: Upload instructions
if ($SkipUpload) {
    Write-Host "Upload skipped. Manual upload required:" -ForegroundColor Yellow
} else {
    Write-Host "Manual upload required:" -ForegroundColor Yellow
}

Write-Host "1. Upload $TargetExe to your web server at: $ServerPath/releases/$Version/"
Write-Host "2. Upload $AppCastPath to your web server at: $ServerPath/"
if (Test-Path "$SignatureDir/$AppName-$Version.exe.signature") {
    Write-Host "3. Upload signature file to: $ServerPath/signatures/"
}
Write-Host ""

# Step 8: Next steps
Write-Host "=== Next Steps ===" -ForegroundColor Green
Write-Host "1. Update the URLs in appcast.xml to match your domain"
Write-Host "2. Upload files to your web server"
Write-Host "3. Test the update by running an older version of the app"
Write-Host "4. Verify the update process works correctly"
Write-Host ""

Write-Host "Deployment preparation completed successfully!" -ForegroundColor Green

