/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package observer.action;

import java.net.DatagramPacket;
import java.net.InetAddress;

import observer.ActionObserver;
import oceanshootertournament_server.dao.FishGamesDAO;
import oceanshootertournament_server.model.Bullet;
import oceanshootertournament_server.model.Float2;

/**
 * @author pc
 */
public class ShootObserver implements ActionObserver {

    @Override
    public void executeAction(DatagramPacket receivePacket) {
        String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());
        String[] receiveArr = receivedData.split(";");
        String idRole = receiveArr[0];
        String action = receiveArr[1];
        String data = receiveArr[2];
        String[] dataArr = data.split(",");
        String roomID = dataArr[0];
        float posX = Float.parseFloat(dataArr[1]);
        float posY = Float.parseFloat(dataArr[2]);
        float deltaX = Float.parseFloat(dataArr[3]);
        float deltaY = Float.parseFloat(dataArr[4]);
        int type = Integer.parseInt(dataArr[6]);
        Bullet bullet = new Bullet(new Float2(posX, posY), new Float2(deltaX, deltaY), "", idRole, type);
        FishGamesDAO.GetFishGames(roomID).ResponseBulletInit(bullet);
    }

}
