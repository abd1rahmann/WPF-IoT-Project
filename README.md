# 🌐 WPF-IoT-Project

**WPF-IoT-Project** is a Windows desktop application that simulates an IoT device (a fan) using WPF and the MVVM pattern. The app can connect to Azure IoT Hub, report its status using Device Twin, and allows users to configure connection settings directly from the UI.

---

## 💼 Tech Stack

- C#
- WPF (.NET 6 or 7)
- MVVM (Model-View-ViewModel)
- Azure IoT Hub
- JSON local storage (for settings)

---

## 🚀 Features

- 🌀 Simulates a smart **fan** with on/off and speed states
- 🔗 Sends telemetry and state to **Azure IoT Hub**
- 🔁 Syncs device state using **Device Twin**
- 🧠 MVVM architecture for clean separation of concerns
- 🛠️ Built-in **settings page** for:
  - Device ID
  - Connection string
  - Local saving of config without database
- 🔄 View switching (Fan ↔ Settings)

---

## 📁 Project Structure

WPF-IoT-Project/
├── Views/ --> Fan view and Settings view (XAML)
├── ViewModels/ --> FanViewModel, SettingsViewModel, MainViewModel
├── Models/ --> Fan model, settings model
├── Services/ --> Azure IoT service, local config service
├── App.xaml --> Application entry point
└── MainWindow.xaml --> Shell with dynamic view switching

---

## 💾 Installation

1. **Clone the repository**
   git clone https://github.com/abd1rahmann/WPF-IoT-Project.git
   cd WPF-IoT-Project
