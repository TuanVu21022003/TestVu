/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package observer.action;

import java.net.DatagramPacket;
import observer.ActionObserver;
import oceanshootertournament_server.thread.JoinRoomThread;
import oceanshootertournament_server.thread.QuitRoomThread;

/**
 *
 * @author pc
 */
public class QuitRoomObserver implements ActionObserver{

    @Override
    public void executeAction(DatagramPacket receivePacket) {
        String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());
        String[] receiveArr = receivedData.split(";");
        String idRole = receiveArr[0];

        new QuitRoomThread(idRole).start();
    }
    
}
