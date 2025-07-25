# Professional Client App

## Description
Professional Client App is a Windows desktop application built using WPF (Windows Presentation Foundation) and the MVVM (Model-View-ViewModel) design pattern. The application provides a client interface to navigate between different property views, such as Handel, Verdi, Olifant, and Coleridge. It is designed to offer a clean and maintainable architecture for managing multiple views and their associated data.

## Getting Started

### Prerequisites
- Windows operating system
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later
- Visual Studio 2022 or later with WPF development workload installed (optional but recommended)

### Building and Running the Application

#### Using Visual Studio
1. Open the solution file `ClienApp.sln` in Visual Studio.
2. Restore NuGet packages if prompted.
3. Build the solution (`Build > Build Solution`).
4. Run the application (`Debug > Start Debugging` or press F5).

#### Using .NET CLI
1. Open a command prompt or terminal.
2. Navigate to the project directory containing `ClienApp.csproj`.
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run --project ClienApp.csproj
   ```

## Usage
The application allows navigation between multiple property views including Handel, Verdi, Olifant, and Coleridge. Use the UI to switch between these views and explore property details.

## Project Structure
- `ClienApp/` - Main project folder containing source code and resources.
  - `App.xaml` and `App.xaml.cs` - Application entry point and configuration.
  - `MainWindow.xaml` and `MainWindow.xaml.cs` - Main window UI and code-behind.
  - `MVVM/` - Contains Model-View-ViewModel folders:
    - `Model/` - Data models (currently minimal or empty).
    - `View/` - XAML views for different properties and home screen.
    - `ViewModel/` - ViewModel classes managing UI logic and navigation.
  - `Core/` - Core utility classes such as `ObservableObject` and `RelayCommand`.
  - `Images/` and `Fonts/` - Static resources used in the UI.
  - `Theme/` - Resource dictionaries for UI theming and styles.

## Features
- Navigation between multiple property views (Handel, Verdi, Olifant, Coleridge).
- MVVM architecture for separation of concerns and maintainability.
- Command-based navigation with back stack support.

## Contributing
Contributions are welcome! Please follow these guidelines:
- Fork the repository and create your feature branch (`git checkout -b feature/your-feature`).
- Commit your changes (`git commit -m 'Add some feature'`).
- Push to the branch (`git push origin feature/your-feature`).
- Open a pull request describing your changes.

## Contact
For support or questions, please open an issue in the repository or contact the project maintainer.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
