/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.model;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.FishGamesDAO;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.thread.GamePlayThread;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;

class UserDataInRoom {
    public int index;
    public float progress;
    public int numberCoin;

    public UserDataInRoom(int index, float progress, int numberCoin) {
        this.index = index;
        this.progress = progress;
        this.numberCoin = numberCoin;
    }

    public JSONObject GetData() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("index", index);
        jsonResponse.put("progress", progress);
        jsonResponse.put("numberCoin", numberCoin);
        return jsonResponse;
    }
}

public class Room {
    private String idRoom;
    private String roomName;
    private List<User> players;
    private HashMap<String, UserDataInRoom> playerDataInRoom;
    private String adminID;

    private List<Integer> indexExistPlayer;
    private final int MAX_PLAYERS = 4; // Tối đa 4 người chơi mỗi phòng

    public Room() {
        this.players = new ArrayList<>();
        playerDataInRoom = new HashMap<>();
        indexExistPlayer = new ArrayList<>();
        for (int i = 0; i < MAX_PLAYERS; i++) {
            indexExistPlayer.add(i);
        }
    }

    public Room(String idRoom, String roomName) {
        this.idRoom = idRoom;
        this.roomName = roomName;
        this.players = new ArrayList<>();
        playerDataInRoom = new HashMap<>();
        indexExistPlayer = new ArrayList<>();
        for (int i = 0; i < MAX_PLAYERS; i++) {
            indexExistPlayer.add(i);
        }
    }

    public String getIdRoom() {
        return idRoom;
    }

    public void setIdRoom(String idRoom) {
        this.idRoom = idRoom;
    }

    public String getAdminID() {
        return adminID;
    }

    public void setAdminID(String adminID) {
        this.adminID = adminID;
    }


    public String getRoomName() {
        return roomName;
    }

    public void setRoomName(String roomName) {
        this.roomName = roomName;
    }

    public List<User> getPlayers() {
        return players;
    }

    public void addPlayer(User user) {
        if (players.size() < MAX_PLAYERS) {
            players.add(user);
        }
    }

    public void removePlayer(String userID) {
        indexExistPlayer.add(playerDataInRoom.get(userID).index);
        playerDataInRoom.remove(userID);
        players.removeIf(user -> user.getUserID().equals(userID));
    }

    public boolean isFull() {
        return players.size() >= MAX_PLAYERS;
    }

    public boolean isEmpty() {
        return players.isEmpty();
    }

    public int getPlayerCount() {
        return players.size();
    }

    public String GetData() {
        String tmp = idRoom + ": ";
        for (int i = 0; i < players.size(); i++) {
            tmp += players.get(i).getUserID() + " ";
        }
        return tmp;
    }

    public int getCountPlayer() {
        return players.size();
    }

    public void AutoSetAdminID() {
        adminID = players.get(0).getUserID();
    }

    public JSONArray GetDataStartGame() {
        JSONArray dataArr = new JSONArray();
        for (int i = 0; i < players.size(); i++) {
            dataArr.add(players.get(i).GetDataStartGame());
        }
        return dataArr;
    }

    public JSONObject GetDataRoom() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("roomID", idRoom);
        jsonResponse.put("roomName", "Phong cua " + UserDAO.GetUser(adminID).getUserName());
        jsonResponse.put("countPlayer", players.size());
        return jsonResponse;
    }

    public JSONObject GetDataJoinRoom() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("roomID", idRoom);
        jsonResponse.put("roomName", idRoom);
        jsonResponse.put("adminID", adminID);
        JSONArray dataArr = new JSONArray();
        for (int i = 0; i < players.size(); i++) {
            dataArr.add(players.get(i).GetData());
        }
        jsonResponse.put("accountDatas", dataArr);

        return jsonResponse;
    }
    
    public JSONArray GetDataEndGame() {
        JSONArray jsonArray = new JSONArray();
        for (int i = 0; i < players.size(); i++) {
            jsonArray.add(players.get(i).GetDataEndGame());
        }
        return jsonArray;
    }

    public void StartGame() {
        playerDataInRoom = new HashMap();
        players.forEach((player) -> {
            var index = indexExistPlayer.remove(0);
            playerDataInRoom.put(player.getUserID(), new UserDataInRoom(index, 0, player.getCoin()));
        });

        players.forEach((player) -> {
            player.setCoin(0);
            JSONObject jsonResponse = new JSONObject();
            jsonResponse.put("idRole", player.getUserID());
            jsonResponse.put("action", "start_game");
            jsonResponse.put("data", GetDataStartGame());
            String mess = jsonResponse.toString();
            OceanShooterTournament_Server.SendResponse(mess, player.getAddress(), player.getPort());
        });
    }

    public void UpdatePlayerProgress(String userID, float progress) {
        if (!playerDataInRoom.containsKey(userID)) {
            return;
        }
        playerDataInRoom.put(userID, new UserDataInRoom(playerDataInRoom.get(userID).index, progress, playerDataInRoom.get(userID).numberCoin));

        for (var userDataInRoom : playerDataInRoom.values()) {
            if (userDataInRoom.progress < 1) {
                return;
            }
        }

        var jsonAllPlayerDataInRoom = new JSONObject();
        playerDataInRoom.forEach((_userID, _userDataInRoom) -> {
            jsonAllPlayerDataInRoom.put(_userID, _userDataInRoom.GetData());
        });
        
        var jsonAllDataInRoom = new JSONObject();
        jsonAllDataInRoom.put("playerDatas", jsonAllPlayerDataInRoom);
        GamePlayThread fishGameThread = new GamePlayThread(idRoom);
        fishGameThread.start();
        FishGamesDAO.AddDictFishGame(idRoom, fishGameThread);
        playerDataInRoom.forEach((_userID, _value) -> {
            var jsonResponse = new JSONObject();
            jsonResponse.put("idRole", _userID);
            jsonResponse.put("action", "all_player_loaded_complete");
            jsonResponse.put("data", jsonAllDataInRoom);
            String mess = jsonResponse.toString();
            OceanShooterTournament_Server.SendResponse(mess, UserDAO.GetUser(_userID).getAddress(), UserDAO.GetUser(_userID).getPort());
        });
    }
}