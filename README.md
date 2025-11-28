# CyberChatBot 🛡️

A secure, intelligent, and interactive cybersecurity chatbot built with ASP.NET Core MVC and MongoDB.

## 📖 Overview

CyberChatBot is designed to help users learn about cybersecurity best practices, manage security-related tasks, and test their knowledge through quizzes. It features a custom-built NLP engine for understanding user intent, a robust task management system, and a secure, modern dark-mode interface.

## ✨ Features

*   **🤖 Intelligent Chatbot**:
    *   **Custom NLP Engine**: Uses Levenshtein distance and Regex for typo-tolerant topic recognition and command parsing.
    *   **Context Aware**: Detects sentiment (Worried, Frustrated, Curious, Happy) and responds appropriately.
    *   **Topics**: Covers Password Safety, Phishing, Privacy, Firewalls, Safe Browsing, and Malware.
*   **📋 Task Management**:
    *   Add tasks via chat commands (e.g., "Remind me to change my password").
    *   Smart scheduling based on user's local timezone.
    *   View, complete, and delete tasks in a dedicated dashboard.
*   **🧠 Interactive Quiz**:
    *   Test your cybersecurity knowledge with a built-in quiz module.
    *   Instant feedback and score tracking.
*   **🔐 User Authentication**:
    *   Session-based login system.
    *   Data isolation: Chat history, tasks, and logs are private to each user.
*   **📊 Activity Logs**:
    *   Tracks all user interactions for review and auditing.
*   **🎨 Modern UI**:
    *   "Cybersecurity Dark Mode" with neon accents.
    *   Responsive design using Bootstrap 5.
    *   Real-time client-side time synchronization.

## 🛠️ Tech Stack

*   **Framework**: ASP.NET Core 8 MVC
*   **Database**: MongoDB (Atlas or Local)
*   **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5.3, Bootstrap Icons
*   **Language**: C#
*   **Tools**: Visual Studio / VS Code

## 🚀 Getting Started

### Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download)
*   [MongoDB](https://www.mongodb.com/try/download/community) (Local or Atlas Connection String)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/yourusername/CyberChatBot.git
    cd CyberChatBot
    ```

2.  **Configure Database**
    *   Open `appsettings.json`.
    *   Update the `ConnectionString` and `DatabaseName` under `MongoDbSettings`.
    ```json
    "MongoDbSettings": {
      "ConnectionString": "mongodb://localhost:27017",
      "DatabaseName": "CyberChatDB"
    }
    ```

3.  **Build the Project**
    ```bash
    dotnet build
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```
    The application will start at `https://localhost:7152` (or similar).

## 💡 Usage Guide

1.  **Login**: Enter any username to access the system (accounts are created automatically).
2.  **Chat**:
    *   Ask questions like *"How do I spot a phishing email?"*
    *   Use commands:
        *   `"Add task scan my PC"`
        *   `"Start quiz"`
        *   `"Show my tasks"`
3.  **Tasks**: Manage your to-do list in the Tasks tab.
4.  **Quiz**: Take the quiz to earn a score.
5.  **Logs**: View your interaction history in the Logs tab.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License.
