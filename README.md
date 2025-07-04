# 🎮 Steam Lobby System (Unity + Mirror + Steamworks.NET)

This project is a **Steam-integrated multiplayer lobby system** built with Unity. It allows players to create lobbies, invite Steam friends, and manage ready states in real-time using Steamworks.NET and Mirror.

## 🧩 Technologies Used

- **Unity** (version 2020.x or later)
- **Mirror** - Networking solution for Unity
- **Steamworks.NET** - C# wrapper for the Steamworks API
- **Steam P2P** - For NAT-punching and direct peer-to-peer communication

## 🚀 Features

- Create and join lobbies using Steam friend invitations
- Real-time player list updates inside the lobby
- Lobby search
- Clean UI synced over the network

## 🔧 Setup Instructions

1. Open the project in Unity Hub.
2. Make sure **Steamworks.NET** is included in the project.
   - If not, download it from [Steamworks.NET GitHub](https://github.com/rlabrecque/Steamworks.NET).
3. Add a `steam_appid.txt` file to your root folder.
   - For testing, use `480` (Spacewar App ID).
4. Run the scene in Unity while **Steam is open**.
5. Host a lobby and invite another player from your Steam friend list or join a lobby with find lobby.

## ▶️ How to Use

1. Launch Steam
2. Run the Unity game (Editor or build)
3. Host creates a lobby
4. Friend joins through Steam invite
5. Both players click "Ready" and the game can begin

## 📁 Project Structure
Assets/
├── Scripts/Steamworks.NET/
│ ├── LobbyController.cs
│ ├── SteamManager.cs
│ ├── PlayerNetwork.cs
│ └── ..... (Other scripts)
├── Prefabs/
└── Scenes/
Packages/
ProjectSettings/

## ❓ Troubleshooting

- **Lobby not visible?**  
  Make sure `steam_appid.txt` is present and Steam is running.

- **Host appears twice?**  
  Double-check how players are added in `OnClientConnect` and `OnServerAddPlayer`.

## 📷 Screenshots
  ![Inviting a steam friend](https://github.com/user-attachments/assets/a87491a4-e526-436e-9760-441f37ee3dbb)
  
  ![Steam friend invited and joined](https://github.com/user-attachments/assets/b2fc80f0-5a8b-44b7-9587-46c9d580d52f)

  

## 💡 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request if you want to improve something or fix a bug.

## 📜 License

GNU 3 License

---

**Developed by:** [Onurkan Ceylan](https://github.com/Onurkan811)  
📫 Contact: onurkanceylan@gmail.com


