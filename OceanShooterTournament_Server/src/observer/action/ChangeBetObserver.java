/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package observer.action;

import java.net.DatagramPacket;
import observer.ActionObserver;
import oceanshootertournament_server.dao.FishGamesDAO;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.thread.CreateRoomThread;

/**
 *
 * @author pc
 */
public class ChangeBetObserver implements ActionObserver{

    @Override
    public void executeAction(DatagramPacket receivePacket) {
        String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());
        String[] receiveArr = receivedData.split(";");
        String idRole = receiveArr[0];
        String data = receiveArr[2];
        String[] dataArr = data.split(",");
        String roomID = dataArr[0];
        int indexBet = Integer.parseInt(dataArr[1]);
        FishGamesDAO.GetFishGames(roomID).ChangeBet(idRole, indexBet);
    }
    
}
