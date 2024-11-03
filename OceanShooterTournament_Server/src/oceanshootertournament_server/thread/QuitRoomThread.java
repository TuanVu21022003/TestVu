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
import org.json.simple.JSONObject;

/**
 *
 * @author pc
 */
public class QuitRoomThread extends Thread {

    private String userID;

    public QuitRoomThread() {
    }

    public QuitRoomThread(String userID) {
        this.userID = userID;
    }

    @Override
    public void run() {
        handleQuitRoom(userID);
    }

    private synchronized void handleQuitRoom(String userID) {
        User userQuit = UserDAO.GetUser(userID);
        Room room = RoomDAO.GetRoom(userQuit.getRoomCurrentID());
        userQuit.setRoomCurrentID("");
        room.removePlayer(userID);
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", userQuit.getUserID());
        jsonResponse.put("action", "quit_room_complete");
        String mess = jsonResponse.toString();
        OceanShooterTournament_Server.SendResponse(mess, userQuit.getAddress(), userQuit.getPort());
        System.out.println(room.isEmpty());
        if (room.isEmpty()) {
            RoomDAO.RemoveDictRoom(room.getIdRoom());
            room = null;
        } else {
            if (room.getAdminID().equals(userID)) {
                room.AutoSetAdminID();
            }
            JSONObject jsonResponseOther = new JSONObject();
            jsonResponseOther.put("idRole", room.getIdRoom());
            jsonResponseOther.put("action", "have_client_quit");
            
            JSONObject jsonDataOther = new JSONObject();            
            jsonDataOther.put("adminID", room.getAdminID());
            jsonDataOther.put("userQuit", userQuit.getUserID());
            
            jsonResponseOther.put("data", jsonDataOther);
            String messOther = jsonResponseOther.toString();
            
            for (int i = 0; i < room.getPlayers().size(); i++) {
                User user = room.getPlayers().get(i);
                OceanShooterTournament_Server.SendResponse(messOther, user.getAddress(), user.getPort());
            }
        }
    }
}
