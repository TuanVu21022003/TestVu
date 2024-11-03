package oceanshootertournament_server.thread;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import observer.Observer;
import oceanshootertournament_server.model.User;

public class UDPRequestHandler extends Thread {

    private DatagramSocket serverSocket;
    private DatagramPacket receivePacket;
    public UDPRequestHandler(DatagramSocket serverSocket, DatagramPacket receivePacket) {
        this.serverSocket = serverSocket;
        this.receivePacket = receivePacket;
    }
    @Override
    public void run() {
        try {
            // Xử lý dữ liệu từ client
            String receivedData = new String(receivePacket.getData(), 0, receivePacket.getLength());

            InetAddress address = receivePacket.getAddress();
            int port = receivePacket.getPort();
            System.out.println("Received from client: " + receivedData + "::::" + address + "+++" + port);
            // Ví dụ: Gửi phản hồi lại cho client
            String[] receiveArr = receivedData.split(";");
            String action = receiveArr[1];
            Observer.getAction(action).executeAction(receivePacket);

        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            // Tài nguyên khác có thể cần được đóng

        }
    }
}
