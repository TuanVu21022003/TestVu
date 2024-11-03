/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import java.util.List;
import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.RoomDAO;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.model.Room;
import oceanshootertournament_server.model.User;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;

/**
 *
 * @author pc
 */
public class GetListRoomThread extends Thread{
    private String userID;
    
    public GetListRoomThread(String userID) {
        this.userID = userID;
    }
    
    @Override
    public void run() {
        List<Room> listRoom = RoomDAO.GetListRoomCurrent();
        System.out.println(userID + " logout success");
        User user = UserDAO.GetUser(userID);      
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", userID);
        jsonResponse.put("action", "get_list_room");
        
        JSONArray dataArray = new JSONArray();
        for(int i = 0; i < listRoom.size(); i++) {
            Room room = listRoom.get(i);
            dataArray.add(room.GetDataRoom());
        }
        jsonResponse.put("data", dataArray);
        String mess = jsonResponse.toString();
        OceanShooterTournament_Server.SendResponse(mess, user.getAddress(), user.getPort());
    }
}
