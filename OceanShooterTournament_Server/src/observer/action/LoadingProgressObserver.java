package observer.action;

import observer.ActionObserver;
import oceanshootertournament_server.thread.LoadingProgressThread;

import java.awt.*;
import java.net.DatagramPacket;

public class LoadingProgressObserver implements ActionObserver {

    @Override
    public void executeAction(DatagramPacket receivePacket) {
        var receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());
        var receiveArr = receivedData.split(";");
        var data = receiveArr[2];
        var userID = receiveArr[0];
        var datas = data.split(",");
        var roomID = datas[0];
        var progress = Float.parseFloat(datas[1]);
        new LoadingProgressThread(userID, roomID, progress).start();
    }
}
