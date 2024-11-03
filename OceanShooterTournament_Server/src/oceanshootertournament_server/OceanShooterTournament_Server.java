/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server;

/**
 *
 * @author ADMIN
 */
import java.io.IOException;
import oceanshootertournament_server.thread.UDPRequestHandler;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.util.logging.Level;
import java.util.logging.Logger;
import observer.Observer;

public class OceanShooterTournament_Server {

    private static final int PORT = 8080;        

    private static DatagramSocket serverSocket;

    public static void main(String[] args) throws SocketException, IOException {
        serverSocket = new DatagramSocket(PORT);
        Observer.Init();
        System.out.println("UDP Server started on port " + PORT);

        while (true) {
            // Nhận yêu cầu từ client
            byte[] receiveBuffer = new byte[1024];
            DatagramPacket receivePacket = new DatagramPacket(receiveBuffer, receiveBuffer.length);
            serverSocket.receive(receivePacket);

            // Xử lý yêu cầu trong một thread mới
            Thread handle = new UDPRequestHandler(serverSocket, receivePacket);
            handle.start();
        }
    }


    public static void SendResponse(String mess, InetAddress address, int port) {
        byte[] sendData = new byte[1024];
        sendData = mess.getBytes();
        DatagramPacket sendPackage = new DatagramPacket(sendData, sendData.length, address, port);
        try {
            serverSocket.send(sendPackage);
        } catch (IOException ex) {
            Logger.getLogger(OceanShooterTournament_Server.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

}
