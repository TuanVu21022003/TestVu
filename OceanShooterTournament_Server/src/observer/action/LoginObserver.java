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

/**
 *
 * @author pc
 */
public class LoginObserver implements ActionObserver {

    @Override
    public void executeAction(DatagramPacket receivePacket) {
        String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());

        InetAddress address = receivePacket.getAddress();
        int port = receivePacket.getPort();
        String[] receiveArr = receivedData.split(";");
        String idRole = receiveArr[0];
        String action = receiveArr[1];
        String data = receiveArr[2];
        String[] dataArr = data.split(",");
        System.out.println(data);
        var username = dataArr[0];
        var password = dataArr[1];
        new LoginThread(new User(username, password, username, username, 1, "avatar001", address, port)).start();
    }

}
