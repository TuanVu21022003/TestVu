/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.model;

import java.net.InetAddress;
import org.json.simple.JSONObject;

/**
 * @author pc
 */
public class User {

    private String username;
    private String password;
    private String userID;
    private String userName;
    private int userLevel;
    private String avatarID;
    private InetAddress address;
    private int port;
    private String roomCurrentID;
    private int coin;

    public User(String username, String password, String userID, String userName, int userLevel, String avatarID, InetAddress address, int port) {
        this.username = username;
        this.password = password;
        this.userID = userID;
        this.userName = userName;
        this.userLevel = userLevel;
        this.avatarID = avatarID;
        this.address = address;
        this.port = port;
        this.roomCurrentID = "";
    }

    public String getRoomCurrentID() {
        return roomCurrentID;
    }

    public void setRoomCurrentID(String roomCurrentID) {
        this.roomCurrentID = roomCurrentID;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getUserID() {
        return userID;
    }

    public void setUserID(String userID) {
        this.userID = userID;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public int getUserLevel() {
        return userLevel;
    }

    public void setUserLevel(int userLevel) {
        this.userLevel = userLevel;
    }

    public String getAvatarID() {
        return avatarID;
    }

    public void setAvatarID(String avatarID) {
        this.avatarID = avatarID;
    }

    public InetAddress getAddress() {
        return address;
    }

    public void setAddress(InetAddress address) {
        this.address = address;
    }

    public int getPort() {
        return port;
    }

    public void setPort(int port) {
        this.port = port;
    }

    public int getCoin() {
        return coin;
    }

    public void setCoin(int coin) {
        this.coin = coin;
    }

    public JSONObject GetData() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("userID", userID);
        jsonResponse.put("userName", userName);
        jsonResponse.put("userLevel", userLevel);
        jsonResponse.put("avatarID", avatarID);
        return jsonResponse;
    }

    public String GetDataJoinRoom() {
        return userID + "|" + userName + "|" + userLevel + "|" + avatarID;
    }

    public JSONObject GetDataStartGame() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("userID", userID);
        jsonResponse.put("userName", userName);
        jsonResponse.put("userLevel", userLevel);
        jsonResponse.put("coin", coin);
        return jsonResponse;
    }
    
    public JSONObject GetDataEndGame() {
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("userID", userID);
        jsonResponse.put("coin", coin);
        return jsonResponse;
    }
    
    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true; // So sánh tham chiếu
        if (obj == null || getClass() != obj.getClass()) return false; // Kiểm tra kiểu đối tượng
        
        User other = (User) obj; // Ép kiểu
        return userID.equals(other.getUserID());
    }
    
    public void ReceiveCoin(int coin) {
        this.coin += coin;
    }
}
