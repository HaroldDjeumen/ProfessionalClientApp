# ClientApp

ClientApp is a WPF desktop application designed for navigation and management of multiple property views. It leverages the MVVM (Model-View-ViewModel) architecture to ensure separation of concerns and maintainability.

## Features

- Navigation between multiple property views including Handel, Verdi, Olifant, and Coleridge.
- MVVM architecture implemented with base classes `ObservableObject` and `RelayCommand` for property change notification and command binding.
- Command-based navigation with back stack support allowing users to navigate back to previous views.
- Note Tab feature that allows users to keep notes saved to a SQLite database.
- Room details loaded dynamically from a SQLite database (`PropertyRooms.db`).
- Image storage managed via a dedicated SQLite database (`ImageStorage.db`).

## Architecture Overview

The application follows the MVVM pattern:

- **Model:** Represents the data and business logic, including room details and notes.
- **View:** XAML files defining the UI for different property views and other screens.
- **ViewModel:** Handles the interaction logic and state for each view. Key ViewModels include:
  - `MainViewModel`: Manages navigation and view state.
  - Property ViewModels: `HandelPropertyViewModel`, `VerdiPropertyViewModel`, `OlifantPropertyViewModel`, `ColeridgePropertyViewModel`.
  - Other ViewModels: `GallaryViewModel`, `NoteViewModel`, `HomeViewModel`, `RoomDetailViewModel`, `PropertiesViewModel`.

Base classes in the `Core` folder provide:
- `ObservableObject`: Implements `INotifyPropertyChanged` for property change notifications.
- `RelayCommand`: Implements `ICommand` for command binding.

## Database Details

The application uses SQLite databases located in the `Data` folder:

- `PropertyRooms.db`: Stores details about rooms including room name, tenant, rent status, and images.
- `Notes.db`: Stores user notes.
- `ImageStorage.db`: Stores images used in the application.

## Build and Run Instructions

1. Ensure you have .NET 8.0 SDK installed.
2. Open the solution `ClientApp.sln` in Visual Studio.
3. Build the solution to restore dependencies and compile the project.
4. Run the application from Visual Studio or by executing the generated `.exe` in the `bin/Debug/net8.0-windows` folder.

---

This README provides an overview of the ClientApp project, its architecture, features, and instructions to build and run the application.
