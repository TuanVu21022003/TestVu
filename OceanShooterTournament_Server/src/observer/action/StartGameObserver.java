/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package observer.action;

import java.net.DatagramPacket;
import java.net.InetAddress;
import observer.ActionObserver;
import oceanshootertournament_server.model.User;
import oceanshootertournament_server.thread.LoginThread;
import oceanshootertournament_server.thread.StartGameThread;

/**
 *
 * @author ADMIN
 */
public class StartGameObserver implements ActionObserver{
    @Override
    public void executeAction(DatagramPacket receivePacket) {
        String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());
        String[] receiveArr = receivedData.split(";");
        String roomID = receiveArr[2];
        new StartGameThread(roomID).start();
    }
}
