/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import java.util.logging.Level;
import java.util.logging.Logger;
import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.model.Room;
import oceanshootertournament_server.model.User;
import org.json.simple.JSONObject;

/**
 *
 * @author pc
 */
public class LogOutThread extends Thread{
    private String userID;

    public LogOutThread(String userID) {
        this.userID = userID;
    }
    
    
    @Override
    public void run() {
        handleLogout(this.userID);
    }
    
    private synchronized void handleLogout(String userID) {
        User user = UserDAO.GetUser(userID);
        if(!user.getRoomCurrentID().equals("")) {
            Thread quitThread = new QuitRoomThread(this.userID); 
            quitThread.start();
            try {
                quitThread.join();
            } catch (InterruptedException ex) {
                Logger.getLogger(LogOutThread.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
        UserDAO.RemoveDictUser(userID);      
        JSONObject jsonResponse = new JSONObject();
        jsonResponse.put("idRole", userID);
        jsonResponse.put("action", "logout_complete");
        String mess = jsonResponse.toString();
        OceanShooterTournament_Server.SendResponse(mess, user.getAddress(), user.getPort());
        user = null;
    }
}
