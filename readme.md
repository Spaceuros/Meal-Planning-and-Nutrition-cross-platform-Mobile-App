# Spejs Meal Planner

A cross-platform mobile application for meal planning and nutrition tracking, built with **.NET MAUI**.

## About the Project
Developed as a final project for the "Mobile Application Development" course, this application is designed to simplify nutrition management through the creation and tracking of daily and weekly menus. 

The application supports multiple platforms (iOS and Android) using a single, unified C# codebase.

## Key Features
* **Meal & Macro Tracking:** Add meals, track daily caloric intake, and monitor macronutrients (Proteins, Carbohydrates, Fats) against personal daily goals.
* **Hydration Tracking:** Built-in water intake tracker with a quick-add interface.
* **User Roles:** Custom workflows for different user types—a "Chef" can prepare and save meals to a menu, while a "Consumer" can easily select and consume those pre-planned meals.
* **Premium UI/UX:** A minimalist, Apple-inspired design language featuring edge-to-edge layouts, strict typographical hierarchy, and custom pill-shaped interactive elements.
* **Global App Settings:** Includes real-time English/Serbian localization and a persistent Light/Dark mode that instantly applies across the app and saves to user preferences.
* **Offline-First:** All data is securely stored locally on the device using a SQLite database.

## Technologies
* **.NET MAUI** (Multi-platform App UI)
* **C#** & **XAML**
* **MVVM** (Model-View-ViewModel) Architecture
* **SQLite-net-pcl** (Local Database)

## Getting Started
1. Clone the repository: 
   `git clone https://github.com/Spaceuros/Meal-Planning-and-Nutrition-cross-platform-Mobile-App.git`
2. Open the project in Visual Studio (or VS Code with the .NET MAUI extension).
3. Run the application on an iOS simulator or Android emulator.

Command to run the project via VS Code on a physical iOS device:
```bash
dotnet build -t:Run -f net9.0-ios -p:RuntimeIdentifier=ios-arm64 -p:_DeviceName="Uros"
