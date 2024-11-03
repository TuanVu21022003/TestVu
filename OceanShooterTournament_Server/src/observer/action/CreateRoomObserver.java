/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package observer.action;

import java.net.DatagramPacket;
import java.net.InetAddress;
import observer.ActionObserver;
import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.thread.CreateRoomThread;

/**
 *
 * @author pc
 */
public class CreateRoomObserver implements ActionObserver{

    @Override
    public void executeAction(DatagramPacket receivePacket) {
        String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());
        String[] receiveArr = receivedData.split(";");
        String idRole = receiveArr[0];
        String action = receiveArr[1];
        new CreateRoomThread(UserDAO.GetUser(idRole)).start();
    }
    
}
