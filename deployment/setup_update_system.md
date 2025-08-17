# ProfessionalClientApp Update System Setup Guide

## Overview

This guide explains how to set up the automatic update system for ProfessionalClientApp using NetSparkle. Once configured, your dad will receive automatic updates without needing new executable files.

## Prerequisites

1. Web hosting space (can be shared hosting, VPS, or cloud storage)
2. .NET 8.0 SDK for building the application
3. NetSparkle tools for generating app casts and signatures

## Step 1: Install NetSparkle Tools

```bash
# Install the NetSparkle command-line tools
dotnet tool install --global NetSparkleUpdater.Tools.AppCastGenerator
```

## Step 2: Generate Security Keys

```bash
# Generate Ed25519 key pair for signing updates
netsparkle-generate-keys --key-type ed25519

# This will create:
# - NetSparkle_Ed25519.priv (private key - keep secret!)
# - NetSparkle_Ed25519.pub (public key - embed in app)
```

**Important:** 
- Keep the private key (`NetSparkle_Ed25519.priv`) secure and never share it
- The public key will be embedded in your application

## Step 3: Configure Application

1. Open `UpdateManager.cs` in your project
2. Update the configuration constants:

```csharp
private const string DEFAULT_APPCAST_URL = "https://yourdomain.com/updates/appcast.xml";
private const string DEFAULT_PUBLIC_KEY = "YOUR_BASE64_PUBLIC_KEY_FROM_STEP_2";
```

Replace:
- `yourdomain.com` with your actual domain
- `YOUR_BASE64_PUBLIC_KEY_FROM_STEP_2` with the content from `NetSparkle_Ed25519.pub`

## Step 4: Set Up Web Server Directory Structure

Create the following directory structure on your web server:

```
/updates/
├── appcast.xml
├── releases/
│   ├── 1.0.0/
│   │   └── ProfessionalClientApp-1.0.0.exe
│   ├── 1.0.1/
│   │   └── ProfessionalClientApp-1.0.1.exe
│   └── ...
└── signatures/
    ├── ProfessionalClientApp-1.0.0.exe.signature
    ├── ProfessionalClientApp-1.0.1.exe.signature
    └── ...
```

## Step 5: Build and Deploy Initial Version

1. Build your application in Release mode:
```bash
dotnet build --configuration Release
```

2. Create installer/executable for distribution
3. Sign the executable:
```bash
netsparkle-generate-signature --private-key NetSparkle_Ed25519.priv --file ProfessionalClientApp.exe
```

4. Upload to web server:
   - Executable to `/updates/releases/1.0.0/`
   - Signature to `/updates/signatures/`

## Step 6: Create App Cast File

Create `appcast.xml` on your web server:

```xml
<?xml version="1.0" encoding="utf-8"?>
<rss version="2.0" xmlns:sparkle="http://www.andymatuschak.org/xml-namespaces/sparkle">
    <channel>
        <title>ProfessionalClientApp Updates</title>
        <link>https://yourdomain.com/updates/appcast.xml</link>
        <description>Updates for ProfessionalClientApp</description>
        <language>en</language>
        
        <item>
            <title>Version 1.0.0</title>
            <description><![CDATA[
                <h3>Initial Release</h3>
                <ul>
                    <li>Enhanced dashboard with quick access buttons</li>
                    <li>Dashboard summary cards</li>
                    <li>Quick actions and notifications</li>
                    <li>Pinned notes and recent activity</li>
                    <li>Automatic update system</li>
                </ul>
            ]]></description>
            <pubDate>Mon, 17 Aug 2025 12:00:00 +0000</pubDate>
            <enclosure url="https://yourdomain.com/updates/releases/1.0.0/ProfessionalClientApp-1.0.0.exe"
                       sparkle:version="1.0.0"
                       sparkle:os="windows"
                       length="10485760"
                       type="application/octet-stream"
                       sparkle:signature="BASE64_SIGNATURE_HERE" />
        </item>
    </channel>
</rss>
```

## Step 7: Automated Build Script

Create a PowerShell script `deploy-update.ps1` for easier deployments:

```powershell
param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    
    [Parameter(Mandatory=$true)]
    [string]$UploadPath
)

# Build the application
Write-Host "Building application..."
dotnet build --configuration Release

# Create release directory
$releaseDir = "releases/$Version"
New-Item -ItemType Directory -Force -Path $releaseDir

# Copy executable
Copy-Item "bin/Release/net8.0-windows/ClientApp.exe" "$releaseDir/ProfessionalClientApp-$Version.exe"

# Generate signature
Write-Host "Generating signature..."
$signature = netsparkle-generate-signature --private-key NetSparkle_Ed25519.priv --file "$releaseDir/ProfessionalClientApp-$Version.exe"

# Update appcast.xml
Write-Host "Updating appcast..."
# Add logic to update appcast.xml with new version

# Upload to server
Write-Host "Uploading to server..."
# Add your upload logic here (FTP, SCP, etc.)

Write-Host "Deployment complete!"
```

## Step 8: Testing the Update System

1. Deploy version 1.0.0 to your dad
2. Create version 1.0.1 with a small change
3. Update appcast.xml with new version info
4. Upload new version to server
5. Test that the application detects and installs the update

## Step 9: Ongoing Maintenance

For each new version:

1. Update version number in `AssemblyInfo.cs` or project file
2. Build the application
3. Run the deployment script
4. Update appcast.xml
5. Upload files to server

## Security Considerations

1. **Private Key Security**: Never commit the private key to version control
2. **HTTPS**: Always use HTTPS for serving updates
3. **Signature Verification**: Never disable signature checking in production
4. **Backup**: Keep backups of your private key in a secure location

## Troubleshooting

### Common Issues:

1. **Update not detected**: Check appcast.xml URL and format
2. **Signature verification failed**: Ensure signature was generated with correct private key
3. **Download fails**: Verify file URLs and server accessibility
4. **Application won't restart**: Check file permissions and antivirus settings

### Debug Mode:

For testing, you can temporarily set `SecurityMode.Unsafe` in `UpdateManager.cs`, but **never use this in production**.

## Benefits for Your Dad

Once set up, your dad will:
- Automatically receive notifications when updates are available
- Download and install updates with minimal interaction
- Always have the latest features and bug fixes
- Never need to manually download and install new versions

## Cost Estimate

- **Web Hosting**: $5-20/month for basic hosting
- **Domain**: $10-15/year (if needed)
- **Setup Time**: 2-4 hours initial setup
- **Maintenance**: 15-30 minutes per update

This one-time setup will save significant time and ensure your dad always has the latest version of the application.

