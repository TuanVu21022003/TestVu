/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import java.util.List;
import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.FishGamesDAO;
import oceanshootertournament_server.dao.RoomDAO;
import oceanshootertournament_server.model.Room;
import oceanshootertournament_server.model.User;
import org.json.simple.JSONObject;

/**
 *
 * @author ADMIN
 */
public class StartGameThread extends Thread {

    private String roomID;

    public StartGameThread(String roomID) {
        this.roomID = roomID;
    }

    @Override
    public void run() {
        
        Room room = RoomDAO.GetRoom(roomID);
        room.StartGame();
    }
}
