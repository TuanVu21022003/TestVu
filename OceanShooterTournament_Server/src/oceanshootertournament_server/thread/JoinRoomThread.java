/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.RoomDAO;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.model.Room;
import oceanshootertournament_server.model.User;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;

/**
 *
 * @author DELL
 */
public class JoinRoomThread extends Thread {

    private String userID;
    private String roomID;

    public JoinRoomThread(String userID, String roomID) {
        this.userID = userID;
        this.roomID = roomID;
    }

    @Override
    public void run() {
        handleJoinRoom(userID, roomID);
    }

    private synchronized void handleJoinRoom(String userID, String roomID) {
        User userJoin = UserDAO.GetUser(userID);
        if (RoomDAO.CheckHaveRoom(roomID)) {
            Room room = RoomDAO.GetRoom(roomID); // Lấy phòng từ danh sách phòng
            if (!room.isFull()) {
                userJoin.setRoomCurrentID(roomID);                

                for (int i = 0; i < room.getPlayers().size(); i++) {
                    User user = room.getPlayers().get(i);
                    JSONObject jsonResponseOther = new JSONObject();
                    jsonResponseOther.put("idRole", user.getUserID());
                    jsonResponseOther.put("action", "new_client_join_room");
                    jsonResponseOther.put("data", userJoin.GetData());
                    String messOther = jsonResponseOther.toString();
                    OceanShooterTournament_Server.SendResponse(messOther, user.getAddress(), user.getPort());
                }
                room.addPlayer(userJoin); // Thêm người dùng vào phòng
                JSONObject jsonResponse = new JSONObject();
                jsonResponse.put("idRole", userJoin.getUserID());
                jsonResponse.put("action", "join_room_complete");
                jsonResponse.put("data", room.GetDataJoinRoom());
                String mess = jsonResponse.toString();
                
                OceanShooterTournament_Server.SendResponse(mess, userJoin.getAddress(), userJoin.getPort());
            } else {
                JSONObject jsonResponse = new JSONObject();
                jsonResponse.put("idRole", userID);
                jsonResponse.put("action", "join_room_fail");
                jsonResponse.put("data", "Phong da day");
                String messageJoin = jsonResponse.toString();
                OceanShooterTournament_Server.SendResponse(messageJoin, userJoin.getAddress(), userJoin.getPort());
            }
        } else {
            JSONObject jsonResponse = new JSONObject();
            jsonResponse.put("idRole", userID);
            jsonResponse.put("action", "join_room_fail");
            jsonResponse.put("data", "Phong khong ton tai");
            String messageJoin = jsonResponse.toString();
            OceanShooterTournament_Server.SendResponse(messageJoin, userJoin.getAddress(), userJoin.getPort());
        }
    }

    private void sendJoinRoomResponse(User user, String message) {
        OceanShooterTournament_Server.SendResponse(message, user.getAddress(), user.getPort());
        System.out.println("Phản hồi từ server: " + message);
    }
}
